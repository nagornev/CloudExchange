using AutoMapper;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Features.Descriptors.Queries.GetAllDescriptors
{
    public class GetAllDescriptorsHandler(IDescriptorRepository _descriptorRepository,
                                          IMapper _mapper)
        : IRequestHandler<GetAllDescriptorsQuery, Result<IEnumerable<DescriptorDto>>>
    {
        public async Task<Result<IEnumerable<DescriptorDto>>> Handle(GetAllDescriptorsQuery request, CancellationToken cancellationToken)
        {
            Result<IEnumerable<DescriptorEntity>> descriptorEntitiesResult = await _descriptorRepository.GetAsync(cancellationToken);

            return descriptorEntitiesResult.IsSuccess ?
                        Result<IEnumerable<DescriptorDto>>.Success(descriptorEntitiesResult.Content.Select(_mapper.Map<DescriptorDto>)) :
                        Result<IEnumerable<DescriptorDto>>.Failure(descriptorEntitiesResult.Error);
        }
    }
}
