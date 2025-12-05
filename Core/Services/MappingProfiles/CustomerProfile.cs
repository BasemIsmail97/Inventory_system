

namespace Services.MappingProfiles
{
    public class CustomerProfile : AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer,CustomerDto>().ReverseMap();
        }
    }
}
