using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;

namespace CloudExchange.FileSystem.Repositories
{
    public class DataRepository : IDataRepository
    {
        private const int _buffer = 4096;

        public async Task<Result<Stream>> GetAsync(DescriptorEntity descriptor, CancellationToken cancellation)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result<Stream>.Failure(Errors.NotFound($"The directory ({descriptor.Path}) does not exist."));

            if (!File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return Result<Stream>.Failure(Errors.InvalidArgument($"The file ({descriptor.Id}) does not exist."));

            Stream stream = new FileStream($"{descriptor.Path}{descriptor.Id}",
                                           FileMode.Open,
                                           FileAccess.Read,
                                           FileShare.Read,
                                           _buffer,
                                           useAsync: true);

            return Result<Stream>.Success(stream);
        }

        public async Task<Result> CreateAsync(DescriptorEntity descriptor, Stream stream, CancellationToken cancellation)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result.Failure(Errors.NotFound($"The directory ({descriptor.Path}) does not exist."));



            using (FileStream file = new FileStream($"{descriptor.Path}{descriptor.Id}",
                                                    FileMode.Create,
                                                    FileAccess.Write,
                                                    FileShare.None,
                                                    _buffer,
                                                    true))
            {
                await stream.CopyToAsync(file, cancellation);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(DescriptorEntity descriptor, CancellationToken cancellation)
        {
            if (!Directory.Exists(descriptor.Path))
                return Result<Stream>.Failure(Errors.NotFound($"The directory ({descriptor.Path}) does not exist."));

            if (!File.Exists($"{descriptor.Path}{descriptor.Id}"))
                return Result<Stream>.Failure(Errors.InvalidArgument($"The file ({descriptor.Id}) does not exist."));

            await Task.WhenAll(Task.Run(() => File.Delete($"{descriptor.Path}{descriptor.Id}"),
                               cancellation));

            return Result.Success();
        }
    }
}
