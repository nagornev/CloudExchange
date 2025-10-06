using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDescriptorDeletedEventService
    {
        Task<Result> HandleAsync(Guid descriptorId, string name, string upload, CancellationToken cancellation = default);
    }
}
