using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.UseCases.Services
{
    public interface IServerFileService
    {
        /// <summary>
        /// Return all desciptors.
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<Descriptor>>> GetDescriptors();

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<Descriptor>>> GetDescriptors(long deathTime);

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<Descriptor>> GetDescriptor(Guid descriptorId);

        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<File>> GetFile(Guid descriptorId);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result> DeleteFile(Guid descriptorId);
    }
}
