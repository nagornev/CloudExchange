using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using System;
using System.Threading.Tasks;
using Stream = System.IO.Stream;

namespace CloudExchange.UseCases.Services
{
    public interface IUserFileService
    {
        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <returns></returns>
        Task<Result<File>> GetFile(Guid descriptorId, string download = null);

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
        Task<Result<Descriptor>> CreateFile(string name, int weight, Stream data, int lifetime, string root = null, string download = null);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        Task<Result> DeleteFile(Guid descriptorId, string root);
    }
}
