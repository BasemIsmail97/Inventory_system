using Shards;
using Shards.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface IProductService
    {
        Task<PagenatedResult<ProductDto>> GetAllProductsAsync(ProductSpecificationParameters specificationParameters);
        Task<ProductDetailsDto> GetProductByIdAsync(int productId);
        Task<int> CreateProductAsync(CreateOrUpdateProductDto createProductDto);
      Task<  int> UpdateProductAsync(CreateOrUpdateProductDto updateProductDto);
       Task< bool> DeleteProductAsync(int productId);
    }
}
