using CloudExchange.Application.Dto;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Descriptors.Queries.GetAllDescriptors
{
    public record GetAllDescriptorsQuery
        : IRequest<Result<IEnumerable<DescriptorDto>>>;
}
