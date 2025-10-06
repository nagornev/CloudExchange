using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class ContinueUploadFileService : IContinueUploadFileService
    {
        private readonly IStorageRepository _storageRepository;

        public ContinueUploadFileService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Result<ContinueUploadDto>> ContinueUploadAsync(string key, string id, int part, CancellationToken cancellation = default)
        {
            string url = await _storageRepository.ContinueUploadAsync(key, id, part, cancellation);

            return Result<ContinueUploadDto>.Success(new ContinueUploadDto(url));
        }
    }
}
