using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public record ContinueUploadFileCommand(string Id, string Key, int Part)
        : IRequest<Result<ContinueUploadDto>>;
}
