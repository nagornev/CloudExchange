using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IAbortUploadFileService
    {
        Task<Result> AbortUploadAsync(string key, string id, CancellationToken cancellation = default);
    }
}
