using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDownloadFileService
    {
        Task<Result<DownloadDto>> DownloadAsync(Guid descriptorId, string? download, CancellationToken cancellation = default);
    }
}
