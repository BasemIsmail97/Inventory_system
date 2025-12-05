

namespace Services.MappingProfiles
{
    public class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDetailsDto, string>
    {
        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;
            return $"{_configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
