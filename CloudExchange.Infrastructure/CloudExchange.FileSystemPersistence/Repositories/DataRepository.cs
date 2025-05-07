using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;

namespace CloudExchange.FileSystemPersistence.Repositories
{
    public class DataRepository : IDataRepository
    {
        private const int _buffer = 4096;

        public Task<Stream> Get(DescriptorEntity descriptor)
        {
            if (!Directory.Exists(descriptor.Path) || 
                !File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return default;

            Stream stream = new FileStream(descriptor.Path,
                                           FileMode.Open,
                                           FileAccess.Read,
                                           FileShare.Read,
                                           _buffer,
                                           useAsync: true);

            return Task.FromResult(stream);
        }

        public async Task<bool> Create(DescriptorEntity descriptor, Stream stream)
        {
            if (!Directory.Exists(descriptor.Path))
                return false;

            using (FileStream file = new FileStream(descriptor.Path,
                                                    FileMode.Create,
                                                    FileAccess.Write,
                                                    FileShare.None,
                                                    _buffer,
                                                    useAsync: true))
            {
                await stream.CopyToAsync(file);
            }

            return true;
        }

        public async Task<bool> Delete(DescriptorEntity descriptor)
        {
            if (!Directory.Exists(descriptor.Path) ||
                !File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return false;

            await Task.WhenAll(Task.Run(() => File.Delete($"{descriptor.Path}{descriptor.Id}")));

            return true;
        }
    }
}
