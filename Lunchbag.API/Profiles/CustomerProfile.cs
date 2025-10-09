using AutoMapper;

namespace Lunchbag.API.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() 
        {
            CreateMap<Entities.Customer, Models.CustomerDto>();
            CreateMap<Models.RegisterCustomerDto, Entities.Customer>();
        }
    }
}
