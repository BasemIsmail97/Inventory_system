

namespace Services.MappingProfiles
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(scr => scr.Category.Name))
                .ForMember(dest => dest.SupplierName, options => options.MapFrom(scr => scr.Supplier.Name));
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(scr => scr.Category.Name))
                .ForMember(dest => dest.SupplierName, options => options.MapFrom(scr => scr.Supplier.Name))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<PictureUrlResolver>());
            CreateMap<CreateOrUpdateProductDto, Product>();
                 




        }
    }
}
