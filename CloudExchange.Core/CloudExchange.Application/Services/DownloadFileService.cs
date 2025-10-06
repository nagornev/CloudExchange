using AutoMapper;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Abstractions.Validators;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Aggregates;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        private readonly IDescriptorQueryService _descriptorQueryService;

        private readonly IDescriptorCredentialsValidator _descriptorCredentialsValidator;

        private readonly IStorageRepository _storageRepository;

        private readonly ITimeProvider _timeProvider;

        private readonly IMapper _mapper;

        public DownloadFileService(IDescriptorQueryService descriptorQueryService,
                                   IDescriptorCredentialsValidator descriptorCredentialsValidator,
                                   IStorageRepository storageRepository,
                                   ITimeProvider timeProvider,
                                   IMapper mapper)
        {
            _descriptorQueryService = descriptorQueryService;
            _descriptorCredentialsValidator = descriptorCredentialsValidator;
            _storageRepository = storageRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        public async Task<Result<DownloadDto>> DownloadAsync(Guid descriptorId, string? download, CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = await IsDownloadAllowed(descriptorId, download, cancellation);

            if (descriptorResult.IsFailure)
                return Result<DownloadDto>.Failure(descriptorResult.Error);

            DescriptorAggregate descriptor = descriptorResult.Content;

            if (!descriptor.IsValidAt(_timeProvider.NowUnix()))
                return Result<DownloadDto>.Failure(ResultError.NotFound($"The file with descriptor id ({descriptorId}) was not found."));

            string? url = await _storageRepository.DownloadAsync(descriptor, cancellation);

            return url != null ?
                        Result<DownloadDto>.Success(new DownloadDto(_mapper.Map<DescriptorDto>(descriptor), url)) :
                        Result<DownloadDto>.Failure(ResultError.NotFound($"The file with descriptor id ({descriptorId}) was not found."));
        }

        private async Task<Result<DescriptorAggregate>> IsDownloadAllowed(Guid descriptorId,
                                                                          string? download,
                                                                          CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = await _descriptorQueryService.GetDescriptorByIdAsync(descriptorId, cancellation);

            if (descriptorResult.IsSuccess &&
                descriptorResult.Content.Credentials?.DownloadHash != null)
                return !string.IsNullOrEmpty(download) ?
                            _descriptorCredentialsValidator.Validate(download,
                                                                     descriptorResult.Content.Credentials.Salt!,
                                                                     descriptorResult.Content.Credentials.DownloadHash) ?
                                descriptorResult :
                                Result<DescriptorAggregate>.Failure(ResultError.InvalidDownload("Invalid download password.")) :
                            Result<DescriptorAggregate>.Failure(ResultError.InvalidDownload("Invalid download password.")) ;


            return descriptorResult;
        }
    }
}
