using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public class ContinueUploadFileHanler : IRequestHandler<ContinueUploadFileCommand, Result<ContinueUploadDto>>
    {
        private readonly IContinueUploadFileService _continueUploadFileService;

        public ContinueUploadFileHanler(IContinueUploadFileService continueUploadFileService)
        {
            _continueUploadFileService = continueUploadFileService;
        }

        public async Task<Result<ContinueUploadDto>> Handle(ContinueUploadFileCommand request,
                                                            CancellationToken cancellationToken)
        {
            return await _continueUploadFileService.ContinueUploadAsync(request.Key, request.Id, request.Part, cancellationToken);
        }
    }
}
