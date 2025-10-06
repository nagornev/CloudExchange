using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Domain.Aggregates;

namespace CloudExchange.Application.Extensions
{
    public static class StorageKeyProviderExtensions
    {
        public static string Get(this IStorageKeyProvider storageKeyProvider, DescriptorAggregate descriptor)
        {
            return storageKeyProvider.Get(descriptor.Id, descriptor.Name);
        }
    }
}
