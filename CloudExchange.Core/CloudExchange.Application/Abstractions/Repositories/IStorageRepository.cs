using CloudExchange.Application.Dto;
using CloudExchange.Domain.Aggregates;

namespace CloudExchange.Application.Abstractions.Repositories
{
    public interface IStorageRepository
    {
        Task<string?> DownloadAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default);

        Task<string> BeginUploadAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default);

        Task<string> ContinueUploadAsync(string key, string id, int part, CancellationToken cancellation = default);

        Task CompleteUploadAsync(string key, string id, IReadOnlyCollection<PartDto> parts, CancellationToken cancellation = default);

        Task AbortUploadAsync(string key, string id, CancellationToken cancellation = default);

        Task DeleteAsync(string key, CancellationToken cancellation = default);
    }
}
