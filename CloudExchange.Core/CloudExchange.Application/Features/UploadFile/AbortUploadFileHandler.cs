using CloudExchange.Application.Abstractions.Services;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public class AbortUploadFileHandler
        : IRequestHandler<AbortUploadFileCommand, Result>
    {
        private readonly IAbortUploadFileService _abortUploadFileService;

        public AbortUploadFileHandler(IAbortUploadFileService abortUploadFileService)
        {
            _abortUploadFileService = abortUploadFileService;
        }

        public async Task<Result> Handle(AbortUploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _abortUploadFileService.AbortUploadAsync(request.Key, request.Id, cancellationToken);
        }
    }
}
