using CloudExchange.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Abstractions.Repositories
{
    public interface IDataRepository
    {
        /// <summary>
        /// Return file stream from the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        Task<Stream> Get(DescriptorEntity descriptor);

        /// <summary>
        /// Creates a file in the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<bool> Create(DescriptorEntity descriptor, Stream stream);

        /// <summary>
        /// Deletes a file from the storage.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        Task<bool> Delete(DescriptorEntity descriptor);
    }
}
