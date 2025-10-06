using AutoMapper;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Aggregates;

namespace CloudExchange.Application.Mappers
{
    public class DescriptorProfile : Profile
    {
        public DescriptorProfile()
        {
            CreateMap<DescriptorAggregate, DescriptorDto>()
                .ConstructUsing(src => new DescriptorDto(src.Id, src.Name, src.Weight, src.CreatedAt, src.ExpiresAt, src.Lifetime));
        }
    }
}
