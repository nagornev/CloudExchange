using CloudExchange.Domain.Aggregates;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDescriptorQueryService
    {
        Task<Result<IReadOnlyCollection<DescriptorAggregate>>> GetAllDescriptorsAsync(CancellationToken cancellation = default);

        Task<Result<DescriptorAggregate>> GetDescriptorByIdAsync(Guid descriptorId, CancellationToken cancellation = default);

        Task<Result<DescriptorAggregate>> GetUnconfimedDescriptorByIdAsync(Guid descriptorId, CancellationToken cancellation = default);

        Task<Result<IReadOnlyCollection<DescriptorAggregate>>> GetExpiredDescriptorsAsync(long timestamp, CancellationToken cancellation = default);
    }
}
