using CloudExchange.Application.Abstractions.Providers;

namespace CloudExchange.Application.Providers
{
    public class StorageKeyProvider : IStorageKeyProvider
    {
        private const char _separator = '/';

        public string Get(Guid id, string name)
        {
            return $"{id}{_separator}{name}";
        }

        public Guid GetId(string key)
        {
            string[] tokens = key.Split(_separator);

            return Guid.Parse(tokens[0]);
        }
    }
}
