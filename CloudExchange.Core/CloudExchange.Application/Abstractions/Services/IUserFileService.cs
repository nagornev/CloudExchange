using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IUserFileService
    {
        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="download"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<FileDto>> GetFileAsync(Guid descriptorId,
                                           string? download = null,
                                           CancellationToken cancellation = default);

        /// <summary>
        /// Creates new file entry in database and file in server.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="weight"></param>
        /// <param name="data"></param>
        /// <param name="lifetime"></param>
        /// <param name="root"></param>
        /// <param name="download"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<DescriptorEntity>> CreateFileAsync(string name,
                                                       int weight,
                                                       Stream data,
                                                       int lifetime,
                                                       string? root = null,
                                                       string? download = null,
                                                       CancellationToken cancellation = default);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="root"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> DeleteFileAsync(Guid descriptorId,
                                     string root,
                                     CancellationToken cancellation = default);
    }
}
