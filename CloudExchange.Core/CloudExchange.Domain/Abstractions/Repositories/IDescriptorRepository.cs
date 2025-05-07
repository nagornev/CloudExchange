using CloudExchange.Domain.Delegates;
using CloudExchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Abstractions.Repositories
{
    public interface IDescriptorRepository
    {
        /// <summary>
        /// Return descriptors for all files.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DescriptorEntity>> Get();

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<DescriptorEntity> Get(Guid descriptorId);

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <returns></returns>
        Task<IAsyncEnumerable<DescriptorEntity>> Get(long deathTime);

        /// <summary>
        /// Creates a new descriptor in the database.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        Task<bool> Create(DescriptorEntity descriptor, TransactionCreateDelegate callback);

        /// <summary>
        /// Deletes descriptor from the database.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task<bool> Delete(DescriptorEntity descriptor, TransactionDeleteDelegate callback);
    }
}
