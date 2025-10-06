using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IBeginUploadFileService
    {
        Task<Result<BeginUploadDto>> BeginUploadAsync(string name, long weight, int lifetime, string? root = null, string? download = null, CancellationToken cancellation = default);
    }
}
