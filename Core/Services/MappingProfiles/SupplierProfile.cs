using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDto>().ReverseMap();
        }
    }
}
