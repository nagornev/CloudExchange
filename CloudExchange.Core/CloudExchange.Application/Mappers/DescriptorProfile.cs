using AutoMapper;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Entities;

namespace CloudExchange.Application.Mappers
{
    public class DescriptorProfile : Profile
    {
        public DescriptorProfile()
        {
            CreateMap<DescriptorEntity, DescriptorDto>();
        }
    }
}
