using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.DeleteFileByUser
{
    public record DeleteFileByUserCommand(Guid DescriptorId,
                                          string Root) 
        : IRequest<Result>;
}
