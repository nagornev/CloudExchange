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
    public interface IUserFileService
    {
        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<FileDto>> GetFile(Guid descriptorId, string? download = null);

        /// <summary>
        /// Creates new file entry in database and file in server.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="weight"></param>
        /// <param name="lifetime"></param>
        /// <param name="data"></param>
        /// <param name="root"></param>
        /// <param name="download"></param>
        /// <returns></returns>
        Task<Result<DescriptorEntity>> CreateFile(string name, int weight, Stream data, int lifetime, string? root = null, string? download = null);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        Task<Result> DeleteFile(Guid descriptorId, string root);
    }
}
