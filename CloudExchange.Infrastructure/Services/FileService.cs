using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Extensions;
using CloudExchange.UseCases.Providers;
using CloudExchange.UseCases.Repositories;
using CloudExchange.UseCases.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = CloudExchange.Domain.Models.File;

namespace CloudExchange.Infrastructure.Services
{
    public class FileService : IUserFileService, IServerFileService
    {
        private readonly IDescriptorRepository _desciptorRepository;

        private readonly IStorageRepository _storageRepository;

        private readonly IHashProvider _hashProvider;

        private readonly ITimeProvider _timeProvider;

        private readonly IPathProvider _pathProvider;

        public FileService(IDescriptorRepository fileRepository,
                           IStorageRepository storageRepository,
                           IHashProvider hashProvider,
                           ITimeProvider timeProvider,
                           IPathProvider pathProvider)
        {
            _desciptorRepository = fileRepository;
            _storageRepository = storageRepository;
            _hashProvider = hashProvider;
            _timeProvider = timeProvider;
            _pathProvider = pathProvider;
        }

        #region ServerFileService

        public async Task<Result<IEnumerable<Descriptor>>> GetDescriptors()
        {
            return await _desciptorRepository.Get();
        }

        public async Task<Result<IAsyncEnumerable<Descriptor>>> GetDescriptors(long deathTime)
        {
            return await _desciptorRepository.Get(deathTime);
        }

        public async Task<Result<Descriptor>> GetDescriptor(Guid descriptorId)
        {
            return await _desciptorRepository.Get(descriptorId);
        }

        public async Task<Result<File>> GetFile(Guid descriptorId)
        {
            Result<Descriptor> descriptorResult = await GetDescriptor(descriptorId);

            return descriptorResult.Success ?
                        await GetFile(descriptorResult.Content) :
                        Result<File>.Failure(descriptorResult);
        }

        public async Task<Result> DeleteFile(Guid descriptorId)
        {
            Result<Descriptor> descriptorResult = await GetDescriptor(descriptorId);

            return descriptorResult.Success ?
                        await DeleteFile(descriptorResult.Content) :
                        Result.Failure(descriptorResult);
        }

        #endregion

        #region UserFileService

        public async Task<Result<File>> GetFile(Guid descriptorId, string? download = null)
        {
            Result<Descriptor> descriptorResult = await IsDownloadAllowed(descriptorId, download);

            return descriptorResult.Success ?
                        await GetFile(descriptorResult.Content) :
                        Result<File>.Failure(descriptorResult);

        }

        public async Task<Result<Descriptor>> CreateFile(string name, int weight, Stream data, int lifetime, string? root = null, string? download = null)
        {
            Result<Descriptor> descriptorResult = Descriptor.New(name, _pathProvider.GetPath(), weight, _timeProvider.NowUnix(), lifetime, root, download, (value) => _hashProvider.Hash(value));

            return descriptorResult.Success ?
                        await CreateFile(descriptorResult.Content, data) :
                        Result<Descriptor>.Failure(descriptorResult);
        }

        public async Task<Result> DeleteFile(Guid descriptorId, string root)
        {
            Result<Descriptor> descriptorResult = await IsRootAllowed(descriptorId, root);

            return descriptorResult.Success ?
                        await DeleteFile(descriptorResult.Content) :
                        Result.Failure(descriptorResult);
        }

        #endregion

        #region Private methods

        private async Task<Result<File>> GetFile(Descriptor descriptor)
        {
            Result<Stream> streamResult = await _storageRepository.Get(descriptor);

            return streamResult.Success ?
                        File.New(descriptor, streamResult.Content) :
                        Result<File>.Failure(streamResult);
        }

        private async Task<Result<Descriptor>> CreateFile(Descriptor descriptor, Stream data)
        {
            Result createResult = await _desciptorRepository.Create(descriptor,
                                                                    async (descriptor) => await _storageRepository.Create(descriptor, data));

            return createResult.Success ?
                        Result<Descriptor>.Successful(descriptor) :
                        Result<Descriptor>.Failure(createResult);
        }

        private async Task<Result> DeleteFile(Descriptor descriptor)
        {
            return await _desciptorRepository.Delete(descriptor,
                                                     async (descriptor) => await _storageRepository.Delete(descriptor));
        }

        private async Task<Result<Descriptor>> IsRootAllowed(Guid descriptorId, string root)
        {
            Result<Descriptor> descriptorResult = await GetDescriptor(descriptorId);

            if (descriptorResult.Success &&
                descriptorResult.Content.Root != null)
                return _hashProvider.Verify(root, descriptorResult.Content.Root) ?
                            descriptorResult :
                            Result<Descriptor>.Failure(error => error.InvalidArgument("Invalid root password."));

            return !descriptorResult.Success ?
                        Result<Descriptor>.Failure(descriptorResult) :
                        Result<Descriptor>.Failure(error => error.NullOrEmpty("This file does`t have a root password."));
        }

        private async Task<Result<Descriptor>> IsDownloadAllowed(Guid descriptorId, string? download)
        {
            Result<Descriptor> descriptorResult = await GetDescriptor(descriptorId);

            if (descriptorResult.Success &&
                descriptorResult.Content.Download != null)
                return _hashProvider.Verify(download, descriptorResult.Content.Download) ?
                            descriptorResult :
                            Result<Descriptor>.Failure(error => error.InvalidArgument("Invalid download password."));

            return descriptorResult;
        }

        #endregion
    }
}
