using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Aggregates;
using CloudExchange.Domain.Specifications;
using DDD.Repositories;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class DescriptorQueryService : IDescriptorQueryService
    {
        private readonly IRepositoryReader<DescriptorAggregate> _descriptorRepository;

        public DescriptorQueryService(IRepositoryReader<DescriptorAggregate> descriptorRepository)
        {
            _descriptorRepository = descriptorRepository;
        }

        public async Task<Result<IReadOnlyCollection<DescriptorAggregate>>> GetAllDescriptorsAsync(CancellationToken cancellation = default)
        {
            DescriptorByConfirmedSpecification specification = new DescriptorByConfirmedSpecification(true);
            IReadOnlyCollection<DescriptorAggregate> descriptors = await _descriptorRepository.FindAsync(specification, cancellation);

            return Result<IReadOnlyCollection<DescriptorAggregate>>.Success(descriptors);
        }

        public async Task<Result<DescriptorAggregate>> GetDescriptorByIdAsync(Guid descriptorId, CancellationToken cancellation = default)
        {
            DescriptorByIdSpecification specificationById = new DescriptorByIdSpecification(descriptorId);
            DescriptorByConfirmedSpecification specificationByConfirmed = new DescriptorByConfirmedSpecification(true);
            DescriptorAggregate? descriptor = await _descriptorRepository.GetAsync(specificationById.And(specificationByConfirmed), cancellation);

            return descriptor != null ?
                   Result<DescriptorAggregate>.Success(descriptor) :
                   Result<DescriptorAggregate>.Failure(ResultError.NotFound($"The descriptor with Id ({descriptorId}) was not found."));
        }

        public async Task<Result<DescriptorAggregate>> GetUnconfimedDescriptorByIdAsync(Guid descriptorId, CancellationToken cancellation = default)
        {
            DescriptorByIdSpecification specificationById = new DescriptorByIdSpecification(descriptorId);
            DescriptorByConfirmedSpecification specificationByConfirmed = new DescriptorByConfirmedSpecification(false);
            DescriptorAggregate? descriptor = await _descriptorRepository.GetAsync(specificationById.And(specificationByConfirmed), cancellation);

            return descriptor != null ?
                   Result<DescriptorAggregate>.Success(descriptor) :
                   Result<DescriptorAggregate>.Failure(ResultError.NotFound($"The descriptor with Id ({descriptorId}) was not found."));
        }

        public async Task<Result<IReadOnlyCollection<DescriptorAggregate>>> GetExpiredDescriptorsAsync(long timestamp, CancellationToken cancellation = default)
        {
            DescriptorByConfirmedSpecification specificationByConfirmed = new DescriptorByConfirmedSpecification(true);
            DescriptorByUploadedBeforeSpecification specificationByUploadedBefore = new DescriptorByUploadedBeforeSpecification(timestamp);

            IReadOnlyCollection<DescriptorAggregate> descriptors = await _descriptorRepository.FindAsync(specificationByConfirmed.And(specificationByUploadedBefore), cancellation);

            return Result<IReadOnlyCollection<DescriptorAggregate>>.Success(descriptors);
        }
    }
}
