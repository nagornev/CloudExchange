using CloudExchange.Application.Abstractions.Services;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.DeleteFile
{
    public class DeleteFileByUserHandler
        : IRequestHandler<DeleteFileByUserCommand, Result>
    {
        private readonly IDeleteFileService _deleteFileService;

        public DeleteFileByUserHandler(IDeleteFileService deleteFileService)
        {
            _deleteFileService = deleteFileService;
        }

        public async Task<Result> Handle(DeleteFileByUserCommand request, CancellationToken cancellationToken)
        {
            return await _deleteFileService.DeleteAsync(request.DescriptorId, request.Root, cancellationToken);
        }
    }
}
