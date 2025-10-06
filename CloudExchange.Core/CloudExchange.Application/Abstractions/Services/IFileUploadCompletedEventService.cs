using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IFileUploadCompletedEventService
    {
        Task<Result> HandleAsync(Guid descriptorId, CancellationToken cancellation = default);
    }
}
