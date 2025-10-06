using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.GetDescriptors
{
    public record GetAllDescriptorsQuery
        : IRequest<Result<IEnumerable<DescriptorDto>>>;
}
