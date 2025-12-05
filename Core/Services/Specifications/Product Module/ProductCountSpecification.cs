

namespace Services.Specifications
{
    public class ProductCountSpecification: BaseSpecifcation<Product>
    {
        public ProductCountSpecification(ProductSpecificationParameters parameters)
            : base(p => (!parameters.CategoryId.HasValue || p.CategoryId == parameters.CategoryId) && (!parameters.SupplierId.HasValue || p.SupplierId == parameters.SupplierId)
            && (string.IsNullOrEmpty(parameters.SearchTerm) || p.Name.ToLower().Contains(parameters.SearchTerm.ToLower())))
        {

        }
    }
}
