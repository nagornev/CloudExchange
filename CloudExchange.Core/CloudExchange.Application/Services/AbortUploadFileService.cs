using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class AbortUploadFileService : IAbortUploadFileService
    {
        private readonly IStorageRepository _storageRepository;

        public AbortUploadFileService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Result> AbortUploadAsync(string key, string id, CancellationToken cancellation = default)
        {
            await _storageRepository.AbortUploadAsync(key, id, cancellation);

            return Result.Success();
        }
    }
}
