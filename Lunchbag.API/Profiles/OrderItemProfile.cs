using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<Entities.OrderItem, Models.OrderItemDto>();
            CreateMap<Models.OrderItemForCreationDto, Entities.OrderItem>();
        }
    }
}
