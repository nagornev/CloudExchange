using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public record AbortUploadFileCommand(string Key, string Id)
        : IRequest<Result>;
}
