using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Delegates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.UseCases.Repositories
{
    public interface IDescriptorRepository
    {
        /// <summary>
        /// Return descriptors for all files.
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<Descriptor>>> Get();

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<Descriptor>> Get(Guid descriptorId);

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <returns></returns>
        Task<Result<IAsyncEnumerable<Descriptor>>> Get(long deathTime);

        /// <summary>
        /// Creates a new descriptor in the database.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        Task<Result> Create(Descriptor descriptor, TransactionCreateDelegate callback);

        /// <summary>
        /// Deletes descriptor from the database.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task<Result> Delete(Descriptor descriptor, TransactionDeleteDelegate callback);
    }
}
