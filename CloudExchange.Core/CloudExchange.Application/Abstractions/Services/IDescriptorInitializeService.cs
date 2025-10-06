using CloudExchange.Domain.Aggregates;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDescriptorInitializeService
    {
        Task InitializeAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default);
    }
}
