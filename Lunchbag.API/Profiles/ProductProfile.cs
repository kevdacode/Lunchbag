using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {

            CreateMap<Entities.Product, Models.ProductDto>();
            CreateMap<Models.ProductForCreationDto, Entities.Product>();
        }
    }
}
