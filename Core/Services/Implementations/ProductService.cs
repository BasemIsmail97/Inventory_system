using AutoMapper;
using Domain.Contract;
using Domain.Entities.ProductModule;
using Services.Abstraction.Contract;
using Services.Specifications;
using Shards;
using Shards.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork ,IMapper _mapper) : IProductService
    {
        public async Task<int> CreateProductAsync(CreateOrUpdateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
             await _unitOfWork.GetRepository<Product>().AddAsync(product);
           return await _unitOfWork.SaveChangesAsync();
        }

        public  async Task< bool> DeleteProductAsync(int productId)
        {
            var Repo =  _unitOfWork.GetRepository<Product>();
            var product = Repo.GetByIdAsync(productId).Result;
            if(product is null)
            {
                return false;
            }
            Repo.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        

        public async Task<PagenatedResult<ProductDto>> GetAllProductsAsync(ProductSpecificationParameters specificationParameters)
        {
            var Repo =  _unitOfWork.GetRepository<Product>();
            var Spec= new ProductWithCategoryAndSupplierSpecification(specificationParameters);
            var Products= Repo.GetAllAsync(Spec);
            var productDtos= _mapper.Map<IEnumerable<ProductDto>>(Products);
            var pageSize = productDtos.Count();
            var CountSpecification = new ProductCountSpecification(specificationParameters);
            var TotalCount = await Repo.CountAsync(CountSpecification);

            return new PagenatedResult<ProductDto>(specificationParameters.PageIndex, pageSize, TotalCount, productDtos);
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int productId)
        {
            var Spec = new ProductWithCategoryAndSupplierSpecification(productId);
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Spec);
            if( product is null)
            {
                throw new KeyNotFoundException($"Product with id {productId} not found.");
            }
            var productDto = _mapper.Map<ProductDetailsDto>(product);
            return productDto;
        }

        public  async Task<int> UpdateProductAsync(CreateOrUpdateProductDto updateProductDto)
        {
           var product = _mapper.Map<Product>(updateProductDto);
            _unitOfWork.GetRepository<Product>().Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }

        

        
    }
}
