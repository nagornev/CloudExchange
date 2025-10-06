using AutoMapper;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Aggregates;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.GetDescriptors
{
    public class GetAllDescriptorsHandler
        : IRequestHandler<GetAllDescriptorsQuery, Result<IEnumerable<DescriptorDto>>>
    {
        private readonly IDescriptorQueryService _descriptorQueryService;

        private readonly IMapper _mapper;

        public GetAllDescriptorsHandler(IDescriptorQueryService descriptorQueryService,
                                        IMapper mapper)
        {
            _descriptorQueryService = descriptorQueryService;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<DescriptorDto>>> Handle(GetAllDescriptorsQuery request, CancellationToken cancellationToken)
        {
            Result<IReadOnlyCollection<DescriptorAggregate>> descriptorsResult = await _descriptorQueryService.GetAllDescriptorsAsync(cancellationToken);

            return descriptorsResult.IsSuccess ?
                        Result<IEnumerable<DescriptorDto>>.Success(descriptorsResult.Content.Select(_mapper.Map<DescriptorDto>)) :
                        Result<IEnumerable<DescriptorDto>>.Failure(descriptorsResult.Error);
        }
    }
}
