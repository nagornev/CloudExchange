using CloudExchange.Application.Dto;
using CloudExchange.OperationResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Features.Descriptors.Queries.GetAllDescriptors
{
    public record GetAllDescriptorsQuery
        : IRequest<Result<IEnumerable<DescriptorDto>>>;
}
