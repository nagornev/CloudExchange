using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IContinueUploadFileService
    {
        Task<Result<ContinueUploadDto>> ContinueUploadAsync(string key, string id, int part, CancellationToken cancellation = default);
    }
}
