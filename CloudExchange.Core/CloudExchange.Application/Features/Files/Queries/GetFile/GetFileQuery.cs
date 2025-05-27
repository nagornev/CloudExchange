using CloudExchange.Application.Dto;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Queries.GetFile
{
    public record GetFileQuery(Guid DescriptorId,
                               string? Download)
        : IRequest<Result<FileDto>>;
}
