using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        public async Task<Result<Stream>> Get(Descriptor descriptor)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result<Stream>.Failure(error => error.NullOrEmpty($"The directory \"{descriptor.Path}\" does`t exist."));

            if (!System.IO.File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return Result<Stream>.Failure(error => error.NullOrEmpty($"The file \"{descriptor.Id}\" does`t exist in the directory \"{descriptor.Path}\"."));

            return Result<Stream>.Successful(System.IO.File.OpenRead($"{descriptor.Path}{descriptor.Id}"));
        }

        public async Task<Result> Create(Descriptor descriptor, Stream stream)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result.Failure(error => error.NullOrEmpty($"The directory \"{descriptor.Path}\" does`t exist."));

            using (FileStream file = System.IO.File.OpenWrite($"{descriptor.Path}{descriptor.Id}"))
            {
                await stream.CopyToAsync(file);
            }

            return Result.Successful();
        }

        public async Task<Result> Delete(Descriptor descriptor)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result.Failure(error => error.NullOrEmpty($"The directory \"{descriptor.Path}\" does`t exist."));

            if (!System.IO.File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return Result.Failure(error => error.NullOrEmpty($"The file \"{descriptor.Id}\" does`t exist in the directory \"{descriptor.Path}\"."));

            await Task.WhenAll(Task.Run(() => System.IO.File.Delete($"{descriptor.Path}{descriptor.Id}")));

            return Result.Successful();
        }
    }
}
