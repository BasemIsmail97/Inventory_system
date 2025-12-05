using Shards.DTOS.CategoryDtos;
using Shards.DTOS.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCatygoriesAsync();
        Task<CustomerDto> GetCategoryByIdAsync(int CustomerId);
        Task<int> CreateCategoryAsync(CustomerDto CreateCustomerDto);
        Task<int> UpdateCategoryAsync(CustomerDto updateCustomerDto);
        Task<bool> DeleteCategoryAsync(int CustomerId);

    }
}
