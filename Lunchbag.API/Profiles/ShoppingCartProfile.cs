using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<Entities.ShoppingCart, Models.ShoppingCartDto>();
            CreateMap<Models.ShoppingCartForCreationDto, Entities.ShoppingCart>();
        }
    }
}
