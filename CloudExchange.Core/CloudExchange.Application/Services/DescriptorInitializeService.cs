using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Aggregates;

namespace CloudExchange.Application.Services
{
    public class DescriptorInitializeService : IDescriptorInitializeService
    {
        private readonly IStorageRepository _storageRepository;

        public DescriptorInitializeService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task InitializeAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default)
        {
            string upload = await _storageRepository.BeginUploadAsync(descriptor, cancellation);

            descriptor.SetUpload(upload);
        }
    }
}
