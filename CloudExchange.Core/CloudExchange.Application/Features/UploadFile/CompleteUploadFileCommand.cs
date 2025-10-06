using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public record CompleteUploadFileCommand(string Id, string Key, IReadOnlyCollection<PartDto> Parts)
        : IRequest<Result>;
}
