using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class DescriptorDeletedEventService : IDescriptorDeletedEventService
    {
        private readonly IStorageRepository _storageRepository;

        private readonly IStorageKeyProvider _storageKeyProvider;

        public DescriptorDeletedEventService(IStorageRepository storageRepository,
                                             IStorageKeyProvider storageKeyProvider)
        {
            _storageRepository = storageRepository;
            _storageKeyProvider = storageKeyProvider;
        }

        public async Task<Result> HandleAsync(Guid descriptorId, string name, string upload, CancellationToken cancellation = default)
        {
            string key = _storageKeyProvider.Get(descriptorId, name);

            if (string.IsNullOrEmpty(upload))
                await _storageRepository.DeleteAsync(key, cancellation);
            else
                await _storageRepository.AbortUploadAsync(key, upload);

            return Result.Success();
        }
    }
}
