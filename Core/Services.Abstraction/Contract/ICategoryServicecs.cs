using Shards;
using Shards.DTOS.CategoryDtos;
using Shards.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface ICategoryServicecs
    {

        Task<IEnumerable<CategoryDto>> GetAllCatygoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int CatygoryId);
        Task<int> CreateCategoryAsync( CategoryDto createCatygoryDto);
        Task<int> UpdateCategoryAsync( CategoryDto updateCatygoryDto);
        Task<bool> DeleteCategoryAsync(int CatygoryId);
    
}
}
