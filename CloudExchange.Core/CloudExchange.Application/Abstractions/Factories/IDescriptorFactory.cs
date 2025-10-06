using CloudExchange.Domain.Aggregates;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Factories
{
    public interface IDescriptorFactory
    {
        Result<DescriptorAggregate> Create(string name, long weight, int lifetime, string? root = null, string? download = null);
    }
}
