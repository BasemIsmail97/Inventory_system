using Shards.DTOS.SpplierDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface ISupplierServicecs
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto> GetSupplierByIdAsync(int SupplierId);
        Task<int> CreateSupplierAsync(SupplierDto createSupplierDto);
        Task<int> UpdateSupplierAsync(SupplierDto updateSupplierDto);
        Task<bool> DeleteSupplierAsync(int SupplierId);
    }
}
