using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface ICompleteUploadFileService
    {
        Task<Result> CompleteUploadAsync(string key, string id, IReadOnlyCollection<PartDto> parts, CancellationToken cancellation = default);
    }
}
