using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using System.IO;
using System.Threading.Tasks;

namespace CloudExchange.UseCases.Repositories
{
    public interface IStorageRepository
    {
        /// <summary>
        /// Return file stream from the storage.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="decriptorId"></param>
        /// <returns></returns>
        Task<Result<Stream>> Get(Descriptor descriptor);

        /// <summary>
        /// Creates a file in the storage.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<Result> Create(Descriptor descriptor, Stream stream);

        /// <summary>
        /// Deletes a file from the storage.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> Delete(Descriptor decriptor);
    }
}
