using CloudExchange.Application.Abstractions.Services;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public class CompleteUploadFileHandler
        : IRequestHandler<CompleteUploadFileCommand, Result>
    {
        private readonly ICompleteUploadFileService _completeUploadFileService;

        public CompleteUploadFileHandler(ICompleteUploadFileService completeUploadFileService)
        {
            _completeUploadFileService = completeUploadFileService;
        }

        public async Task<Result> Handle(CompleteUploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _completeUploadFileService.CompleteUploadAsync(request.Key, request.Id, request.Parts, cancellationToken);
        }
    }
}
