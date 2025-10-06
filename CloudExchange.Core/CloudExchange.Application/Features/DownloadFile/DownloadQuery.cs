using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.DownloadFile
{
    public record DownloadQuery(Guid DescriptorId,
                                string? Download)
        : IRequest<Result<DownloadDto>>;
}
