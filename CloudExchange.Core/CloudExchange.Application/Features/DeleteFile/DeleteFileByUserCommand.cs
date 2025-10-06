using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.DeleteFile
{
    public record DeleteFileByUserCommand(Guid DescriptorId,
                                          string Root)
        : IRequest<Result>;
}
