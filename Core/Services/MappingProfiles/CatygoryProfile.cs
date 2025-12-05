

namespace Services.MappingProfiles
{
    public class CatygoryProfile : Profile
    {
        public CatygoryProfile()
        {
          CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
