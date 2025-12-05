using Shards;
using Shards.DTOS.ProductDtos;
using Shards.DTOS.PurchaseOrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface IPurchaseOrderService
    {
        Task<PagenatedResult<PurchaseOrderDto>> GetAllPurchaseOrderAsync(PurchaseOrderSpecificationParameters specificationParameters);
        Task<PurchaseDetailsDto> GetPurchaseOrderByIdAsync(int PurchaseOrderId);
        Task<int> CreatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderDto createPurchaseOrderDto);
        Task<int> UpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderDto updatePurchaseOrderDto);
        Task<bool> DeletePurchaseOrderAsync(int PurchaseOrderId);
    }
}
