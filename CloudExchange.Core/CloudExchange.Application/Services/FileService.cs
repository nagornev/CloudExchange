using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Extensions;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;

namespace CloudExchange.Application.Services
{
    public class FileService : IUserFileService, IServerFileService
    {
        private readonly IDescriptorRepository _desciptorRepository;

        private readonly IDataRepository _dataRepository;

        private readonly IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider;

        private readonly ITimeProvider _timeProvider;

        private readonly IPathProvider _pathProvider;

        public FileService(IDescriptorRepository fileRepository,
                           IDataRepository dataRepository,
                           IDescriptorCredentialsHashProvider descriptorCredentialsHashProvider,
                           ITimeProvider timeProvider,
                           IPathProvider pathProvider)
        {
            _desciptorRepository = fileRepository;
            _dataRepository = dataRepository;
            _descriptorCredentialsHashProvider = descriptorCredentialsHashProvider;
            _timeProvider = timeProvider;
            _pathProvider = pathProvider;
        }

        #region ServerFileService

        public async Task<Result<IEnumerable<DescriptorEntity>>> GetDescriptors()
        {
            IEnumerable<DescriptorEntity> descriptors = await _desciptorRepository.Get();

            return Result<IEnumerable<DescriptorEntity>>.Successful(descriptors);
        }

        public async Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetDescriptors(long deathTime)
        {
            IAsyncEnumerable<DescriptorEntity> descriptors = await _desciptorRepository.Get(deathTime);

            return Result<IAsyncEnumerable<DescriptorEntity>>.Successful(descriptors);
        }

        public async Task<Result<DescriptorEntity>> GetDescriptor(Guid descriptorId)
        {
            DescriptorEntity? descriptor = await _desciptorRepository.Get(descriptorId);

            return descriptor != null ?
                    Result<DescriptorEntity>.Successful(descriptor) :
                    Result<DescriptorEntity>.Failure(builder => builder.NullOrEmpty($"The file {descriptorId} was not found."));
        }

        public async Task<Result<FileDto>> GetFile(Guid descriptorId)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptor(descriptorId);

            return descriptorResult.Success ?
                        await GetFile(descriptorResult.Content) :
                        Result<FileDto>.Failure(descriptorResult);
        }

        public async Task<Result> DeleteFile(Guid descriptorId)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptor(descriptorId);

            return descriptorResult.Success ?
                        await DeleteFile(descriptorResult.Content) :
                        Result.Failure(descriptorResult);
        }

        #endregion

        #region UserFileService

        public async Task<Result<FileDto>> GetFile(Guid descriptorId, string? download = null)
        {
            Result<DescriptorEntity> descriptorResult = await IsDownloadAllowed(descriptorId, download);

            return descriptorResult.Success ?
                        await GetFile(descriptorResult.Content) :
                        Result<FileDto>.Failure(descriptorResult);

        }

        public async Task<Result<DescriptorEntity>> CreateFile(string name, int weight, Stream data, int lifetime, string? root = null, string? download = null)
        {
            Result<DescriptorEntity> descriptorResult = DescriptorEntity.New(name,
                                                                             _pathProvider.GetPath(),
                                                                             weight,
                                                                             _timeProvider.NowUnix(),
                                                                             lifetime,
                                                                             root,
                                                                             download,
                                                                             _descriptorCredentialsHashProvider);

            return descriptorResult.Success ?
                        await CreateFile(descriptorResult.Content, data) :
                        Result<DescriptorEntity>.Failure(descriptorResult);
        }

        public async Task<Result> DeleteFile(Guid descriptorId, string root)
        {
            Result<DescriptorEntity> descriptorResult = await IsRootAllowed(descriptorId, root);

            return descriptorResult.Success ?
                        await DeleteFile(descriptorResult.Content) :
                        Result.Failure(descriptorResult);
        }

        #endregion

        #region Private methods

        private async Task<Result<FileDto>> GetFile(DescriptorEntity descriptor)
        {
            Stream? stream = await _dataRepository.Get(descriptor);

            return stream != null ?
                    FileDto.Constructor(descriptor, stream) :
                    Result<FileDto>.Failure(builder => builder.NullOrEmpty($"The file {descriptor.Id} was not found."));
        }

        private async Task<Result<DescriptorEntity>> CreateFile(DescriptorEntity descriptor, Stream data)
        {
            bool success = await _desciptorRepository.Create(descriptor,
                                                             async (descriptor) => await _dataRepository.Create(descriptor, data));

            return success ?
                        Result<DescriptorEntity>.Successful(descriptor) :
                        Result<DescriptorEntity>.Failure(builder => builder.Operation($"The file {descriptor.Name} was not created."));
        }

        private async Task<Result> DeleteFile(DescriptorEntity descriptor)
        {
            bool success = await _desciptorRepository.Delete(descriptor,
                                                             async (descriptor) => await _dataRepository.Delete(descriptor));

            return success ?
                      Result.Successful() :
                      Result.Failure(builder => builder.Operation($"The file {descriptor.Id} was not deleted."));
        }

        private async Task<Result<DescriptorEntity>> IsRootAllowed(Guid descriptorId, string root)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptor(descriptorId);

            if (descriptorResult.Success &&
                descriptorResult.Content.Credentials.Root != null)
                return _descriptorCredentialsHashProvider.Verify(root, descriptorResult.Content.Credentials.Root) ?
                            descriptorResult :
                            Result<DescriptorEntity>.Failure(error => error.InvalidArgument("Invalid root password."));

            return !descriptorResult.Success ?
                        Result<DescriptorEntity>.Failure(descriptorResult) :
                        Result<DescriptorEntity>.Failure(error => error.NullOrEmpty("This file does`t have a root password."));
        }

        private async Task<Result<DescriptorEntity>> IsDownloadAllowed(Guid descriptorId, string? download)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptor(descriptorId);

            if (descriptorResult.Success &&
                descriptorResult.Content.Credentials.Download != null)
                return _descriptorCredentialsHashProvider.Verify(download, descriptorResult.Content.Credentials.Download) ?
                            descriptorResult :
                            Result<DescriptorEntity>.Failure(error => error.InvalidArgument("Invalid download password."));

            return descriptorResult;
        }

        #endregion
    }
}
