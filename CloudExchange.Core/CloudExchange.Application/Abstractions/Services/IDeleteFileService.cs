using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDeleteFileService
    {
        Task<Result> DeleteAsync(Guid descriptorId, string root, CancellationToken cancellation = default);
    }
}
