using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Abstractions.Repositories
{
    public interface IDataRepository
    {
        /// <summary>
        /// Return file stream from the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<Stream>> GetAsync(DescriptorEntity descriptor,
                                      CancellationToken cancellation = default);

        /// <summary>
        /// Creates a file in the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="stream"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> CreateAsync(DescriptorEntity descriptor,
                                 Stream stream,
                                 CancellationToken cancellation = default);

        /// <summary>
        /// Deletes a file from the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> DeleteAsync(DescriptorEntity descriptor,
                                 CancellationToken cancellation = default);
    }
}
