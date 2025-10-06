using CloudExchange.Application.Abstractions.Factories;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Extensions;
using CloudExchange.Domain.Aggregates;
using DDD.Repositories;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class BeginUploadFileService : IBeginUploadFileService
    {

        private readonly IDescriptorFactory _descriptorFactory;

        private readonly IDescriptorInitializeService _descriptorInitializeService;

        private readonly IRepositoryWriter<DescriptorAggregate> _descriptorRepository;

        private readonly IStorageKeyProvider _storageKeyProvider;

        private readonly IUnitOfWork _unitOfWork;

        public BeginUploadFileService(IDescriptorFactory descriptorFactory,
                                      IDescriptorInitializeService descriptorInitializeService,
                                      IRepositoryWriter<DescriptorAggregate> descriptorRepository,
                                      IStorageKeyProvider storageKeyProvider,
                                      IUnitOfWork unitOfWork)
        {
            _descriptorFactory = descriptorFactory;
            _descriptorInitializeService = descriptorInitializeService;
            _descriptorRepository = descriptorRepository;
            _storageKeyProvider = storageKeyProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BeginUploadDto>> BeginUploadAsync(string name, long weight, int lifetime, string? root = null, string? download = null, CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = _descriptorFactory.Create(name, weight, lifetime, root, download);

            if (descriptorResult.IsFailure)
                return Result<BeginUploadDto>.Failure(descriptorResult.Error);

            DescriptorAggregate descriptor = descriptorResult.Content;

            await _descriptorInitializeService.InitializeAsync(descriptor);

            await _descriptorRepository.AddAsync(descriptor, cancellation);
            await _unitOfWork.SaveAsync(cancellation);

            return Result<BeginUploadDto>.Success(new BeginUploadDto(descriptor.Upload,
                                                                     _storageKeyProvider.Get(descriptor),
                                                                     descriptor.Id));
        }
    }
}
