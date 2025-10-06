using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class CompleteUploadFileService : ICompleteUploadFileService
    {
        private readonly IStorageRepository _storageRepository;

        public CompleteUploadFileService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Result> CompleteUploadAsync(string key, string id, IReadOnlyCollection<PartDto> parts, CancellationToken cancellation = default)
        {
            await _storageRepository.CompleteUploadAsync(key, id, parts, cancellation);

            return Result.Success();
        }
    }
}
