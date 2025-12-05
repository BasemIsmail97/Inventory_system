

namespace Services.MappingProfiles
{
    public class SalesOrderProfile : Profile
    {
        public SalesOrderProfile()
        {
            CreateMap<SalesOrder, SalesOrderDto>().ReverseMap();
            CreateMap<SalesOrder, CreateOrUpdateSalesOrderDto>().ReverseMap();
            CreateMap<SalesOrder, SalesDetailsDto>().ReverseMap();
            CreateMap<SalesOrderDetail, SalesOrderDetailDto>().ReverseMap();
        }
    }
}
