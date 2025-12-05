



namespace Services.MappingProfiles
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<PurchaseOrder, CreateOrUpdatePurchaseOrderDto>().ReverseMap();
            CreateMap<PurchaseOrder, PurchaseDetailsDto>().ReverseMap();
            CreateMap<PurchaseOrderDetail, PurchaseOrderDetailDto>().ReverseMap();
           
        }

    }
}
