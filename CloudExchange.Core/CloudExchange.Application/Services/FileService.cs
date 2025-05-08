using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Extensions;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
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

        public async Task<Result<IEnumerable<DescriptorEntity>>> GetDescriptorsAsync(CancellationToken cancellation = default)
        {
            return await _desciptorRepository.GetAsync(cancellation);
        }

        public async Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetDescriptorsAsync(long deathTime,
                                                                                          CancellationToken cancellation = default)
        {
            return await _desciptorRepository.GetAsync(deathTime, cancellation);
        }

        public async Task<Result<DescriptorEntity>> GetDescriptorAsync(Guid descriptorId,
                                                                       CancellationToken cancellation = default)
        {
            return await _desciptorRepository.GetAsync(descriptorId, cancellation);
        }

        public async Task<Result<FileDto>> GetFileAsync(Guid descriptorId,
                                                        CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptorAsync(descriptorId, cancellation);

            return descriptorResult.IsSuccess ?
                        await GetFile(descriptorResult.Content) :
                        Result<FileDto>.Failure(descriptorResult.Error);
        }

        public async Task<Result> DeleteFileAsync(Guid descriptorId,
                                                  CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptorAsync(descriptorId, cancellation);

            return descriptorResult.IsSuccess ?
                        await DeleteFile(descriptorResult.Content) :
                        Result.Failure(descriptorResult.Error);
        }

        #endregion

        #region UserFileService

        public async Task<Result<FileDto>> GetFileAsync(Guid descriptorId,
                                                        string? download = null,
                                                        CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await IsDownloadAllowed(descriptorId, download, cancellation);

            return descriptorResult.IsSuccess ?
                        await GetFile(descriptorResult.Content) :
                        Result<FileDto>.Failure(descriptorResult.Error);

        }

        public async Task<Result<DescriptorEntity>> CreateFileAsync(string name,
                                                                    int weight,
                                                                    Stream data,
                                                                    int lifetime,
                                                                    string? root = null,
                                                                    string? download = null,
                                                                    CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = DescriptorEntity.New(name,
                                                                             _pathProvider.GetPath(),
                                                                             weight,
                                                                             _timeProvider.NowUnix(),
                                                                             lifetime,
                                                                             root,
                                                                             download,
                                                                             _descriptorCredentialsHashProvider);

            return descriptorResult.IsSuccess ?
                        await CreateFile(descriptorResult.Content, data, cancellation) :
                        descriptorResult;
        }

        public async Task<Result> DeleteFileAsync(Guid descriptorId,
                                                  string root,
                                                  CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await IsRootAllowed(descriptorId, root);

            return descriptorResult.IsSuccess ?
                        await DeleteFile(descriptorResult.Content, cancellation) :
                        descriptorResult;
        }

        #endregion

        #region Private methods

        private async Task<Result<FileDto>> GetFile(DescriptorEntity descriptor,
                                                    CancellationToken cancellation = default)
        {
            Result<Stream> dataResult = await _dataRepository.GetAsync(descriptor, cancellation);

            return dataResult.IsSuccess ?
                    FileDto.Create(descriptor, dataResult.Content) :
                    Result<FileDto>.Failure(dataResult.Error);
        }

        private async Task<Result<DescriptorEntity>> CreateFile(DescriptorEntity descriptor,
                                                                Stream data,
                                                                CancellationToken cancellation = default)
        {
            Result createResult = await _desciptorRepository.CreateAsync(descriptor,
                                                                         async (descriptor, cancellation) => await _dataRepository.CreateAsync(descriptor, data, cancellation),
                                                                         cancellation);

            return createResult.IsSuccess ?
                        Result<DescriptorEntity>.Success(descriptor) :
                        Result<DescriptorEntity>.Failure(createResult.Error);
        }

        private async Task<Result> DeleteFile(DescriptorEntity descriptor,
                                              CancellationToken cancellation = default)
        {
            Result deleteResult = await _desciptorRepository.DeleteAsync(descriptor,
                                                             async (descriptor, cancellation) => await _dataRepository.DeleteAsync(descriptor, cancellation),
                                                             cancellation);

            return deleteResult.IsSuccess ?
                      Result.Success() :
                      Result.Failure(deleteResult.Error);
        }

        private async Task<Result<DescriptorEntity>> IsRootAllowed(Guid descriptorId,
                                                                   string root,
                                                                   CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptorAsync(descriptorId, cancellation);

            if (descriptorResult.IsSuccess &&
                descriptorResult.Content.Credentials.Root != null)
                return _descriptorCredentialsHashProvider.Verify(root, descriptorResult.Content.Credentials.Root) ?
                            descriptorResult :
                            Result<DescriptorEntity>.Failure(Errors.InvalidRoot("Invalid root password."));

            return descriptorResult.IsFailure?
                    descriptorResult:
                    Result<DescriptorEntity>.Failure(Errors.InvalidRoot("This file does`t have a root password."));
        }

        private async Task<Result<DescriptorEntity>> IsDownloadAllowed(Guid descriptorId,
                                                                       string? download,
                                                                       CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await GetDescriptorAsync(descriptorId, cancellation);

            if (descriptorResult.IsSuccess &&
                descriptorResult.Content.Credentials.Download != null)
                return _descriptorCredentialsHashProvider.Verify(download, descriptorResult.Content.Credentials.Download) ?
                            descriptorResult :
                            Result<DescriptorEntity>.Failure(Errors.InvalidDownload("Invalid download password."));

            return descriptorResult;
        }

        #endregion
    }
}
