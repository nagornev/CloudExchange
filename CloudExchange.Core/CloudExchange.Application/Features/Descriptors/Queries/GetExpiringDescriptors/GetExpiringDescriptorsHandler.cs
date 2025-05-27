using AutoMapper;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Descriptors.Queries.GetExpiringDescriptors
{
    public class GetExpiringDescriptorsHandler(IDescriptorRepository _descriptorRepository,
                                               IMapper _mapper)
        : IRequestHandler<GetExpiringDescriptorsQuery, Result<IAsyncEnumerable<DescriptorDto>>>
    {
        public async Task<Result<IAsyncEnumerable<DescriptorDto>>> Handle(GetExpiringDescriptorsQuery request, CancellationToken cancellationToken)
        {
            Result<IAsyncEnumerable<DescriptorEntity>> descriptorEntitiesResult = await _descriptorRepository.GetAsync(request.ExpiringTime, cancellationToken);

            return descriptorEntitiesResult.IsSuccess ?
                    Result<IAsyncEnumerable<DescriptorDto>>.Success(GetDescriptorDtos(descriptorEntitiesResult.Content)) :
                    Result<IAsyncEnumerable<DescriptorDto>>.Failure(descriptorEntitiesResult.Error);
        }

        private async IAsyncEnumerable<DescriptorDto> GetDescriptorDtos(IAsyncEnumerable<DescriptorEntity> descriptorEntities)
        {
            await foreach(DescriptorEntity descriptorEntity in descriptorEntities)
            {
                yield return _mapper.Map<DescriptorDto>(descriptorEntity);
            }
        }
    }
}
