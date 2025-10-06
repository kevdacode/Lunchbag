using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() {
            CreateMap<Entities.Category, Models.CategoryDto>();
            CreateMap<Models.CategoryDto, Entities.Category>();
        }
    }
}
