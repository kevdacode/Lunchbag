    using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {
            CreateMap<Entities.Order, Models.OrderDto>();
            CreateMap<Models.OrderForCreationDto, Entities.Order>();
        }
    }
}
