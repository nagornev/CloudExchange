using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Application.Features.Files.Commands.DeleteExpiredFile;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Services.Features.Files
{
    public class DeleteFileByServerService : IDeleteFileByServerService
    {
        private readonly IMediator _mediator;

        public DeleteFileByServerService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> DeleteFile(Guid descriptorId, CancellationToken cancellation = default)
        {
            return await _mediator.Send(new DeleteFileByServerCommand(descriptorId), cancellation);
        }
    }
}
