using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.DeleteExpiredFile
{
    public record DeleteFileByServerCommand(Guid DescriptorId)
        : IRequest<Result>;
}
