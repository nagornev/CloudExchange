using CloudExchange.Domain.Abstractions.Delegates;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Abstractions.Repositories
{
    public interface IDescriptorRepository
    {
        /// <summary>
        /// Return descriptors for all files.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<DescriptorEntity>>> GetAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<DescriptorEntity>> GetAsync(Guid descriptorId,
                                                CancellationToken cancellation = default);

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetAsync(long deathTime,
                                                                  CancellationToken cancellation = default);

        /// <summary>
        /// Creates a new descriptor in the database.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> CreateAsync(DescriptorEntity descriptor,
                                 TransactionCreateAsyncDelegate callback,
                                 CancellationToken cancellation = default);

        /// <summary>
        /// Deletes descriptor from the database.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> DeleteAsync(DescriptorEntity descriptor,
                                 TransactionDeleteAsyncDelegate callback,
                                 CancellationToken cancellation = default);
    }
}
