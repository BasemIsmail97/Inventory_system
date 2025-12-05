using Shards;
using Shards.DTOS.PurchaseOrderDtos;
using Shards.DTOS.SalesOrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface ISalesOrderService
    {
        Task<PagenatedResult<SalesOrderDto>> GetAllSalesOrderAsync(SalesOrderSpecificationParameters specificationParameters);
        Task<SalesDetailsDto> GetSalesOrderByIdAsync(int SalesOrderId);
        Task<int> CreateSalesOrderAsync(CreateOrUpdateSalesOrderDto createSalesOrderDto);
        Task<int> UpdateSalesOrderAsync(CreateOrUpdateSalesOrderDto updateSaleseOrderDto);
        Task<bool> DeleteSalesOrderAsync(int SalesOrderId);
    }
}
