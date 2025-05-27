using CloudExchange.Application.Dto;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Descriptors.Queries.GetExpiringDescriptors
{
    public record GetExpiringDescriptorsQuery(long ExpiringTime)
        : IRequest<Result<IAsyncEnumerable<DescriptorDto>>>;
}
