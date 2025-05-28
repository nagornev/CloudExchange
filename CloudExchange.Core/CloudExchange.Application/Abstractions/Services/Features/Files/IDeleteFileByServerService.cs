using CloudExchange.OperationResults;

namespace CloudExchange.Application.Abstractions.Services.Features.Files
{
    public interface IDeleteFileByServerService
    {
        Task<Result> DeleteFile(Guid descriptorId, CancellationToken cancellation = default);
    }
}
