using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IServerFileService
    {
        /// <summary>
        /// Return all desciptors.
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<DescriptorEntity>>> GetDescriptors();

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <returns></returns>
        Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetDescriptors(long deathTime);

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<DescriptorEntity>> GetDescriptor(Guid descriptorId);

        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<FileDto>> GetFile(Guid descriptorId);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result> DeleteFile(Guid descriptorId);
    }
}
