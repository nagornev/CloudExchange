using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.DownloadFile
{
    public class DownloadFileHandler
        : IRequestHandler<DownloadQuery, Result<DownloadDto>>
    {
        private readonly IDownloadFileService _downloadFileService;

        public DownloadFileHandler(IDownloadFileService downloadFileService)
        {
            _downloadFileService = downloadFileService;
        }

        public async Task<Result<DownloadDto>> Handle(DownloadQuery request, CancellationToken cancellationToken)
        {
            return await _downloadFileService.DownloadAsync(request.DescriptorId, request.Download, cancellationToken);
        }
    }
}
