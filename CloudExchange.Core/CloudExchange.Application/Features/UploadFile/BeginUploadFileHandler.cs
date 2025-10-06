using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public class BeginUploadFileHandler
        : IRequestHandler<BeginUploadFileCommand, Result<BeginUploadDto>>
    {
        private readonly IBeginUploadFileService _createFileService;

        public BeginUploadFileHandler(IBeginUploadFileService createFileService)
        {
            _createFileService = createFileService;
        }

        public async Task<Result<BeginUploadDto>> Handle(BeginUploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _createFileService.BeginUploadAsync(request.Name, request.Weight, request.Lifetime, request.Root, request.Download, cancellationToken);
        }
    }
}
