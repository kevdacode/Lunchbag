using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class ShoppingCartItemProfile : Profile
    {
        public ShoppingCartItemProfile()
        {
            CreateMap<Entities.ShoppingCartItem, Models.ShoppingCartItemDto>();
            CreateMap<Models.ShoppingCartItemForCreationDto, Entities.ShoppingCartItem>();
        }
    }
}
