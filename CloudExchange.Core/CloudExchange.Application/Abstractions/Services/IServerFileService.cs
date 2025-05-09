using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface IServerFileService
    {
        /// <summary>
        /// Return all desciptors.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<DescriptorEntity>>> GetDescriptorsAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Return descriptors about files, that will be dead in <paramref name="deathTime"/>.
        /// </summary>
        /// <param name="deathTime"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetDescriptorsAsync(long deathTime,
                                                                             CancellationToken cancellation = default);

        /// <summary>
        /// Return file descriptor.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<DescriptorEntity>> GetDescriptorAsync(Guid descriptorId,
                                                          CancellationToken cancellation = default);

        /// <summary>
        /// Return the saved file by the <paramref name="descriptorId"/>.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result<FileDto>> GetFileAsync(Guid descriptorId,
                                           CancellationToken cancellation = default);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="descriptorId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<Result> DeleteFileAsync(Guid descriptorId,
                                     CancellationToken cancellation = default);
    }
}
