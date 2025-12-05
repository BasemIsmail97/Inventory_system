

namespace Services.Specifications
{
    public class ProductWithCategoryAndSupplierSpecification : BaseSpecifcation<Product>
    {
        public ProductWithCategoryAndSupplierSpecification(ProductSpecificationParameters parameters)
            : base(p => (!parameters.CategoryId.HasValue || p.CategoryId == parameters.CategoryId) && (!parameters.SupplierId.HasValue || p.SupplierId == parameters.SupplierId)
            && (string.IsNullOrEmpty(parameters.SearchTerm) || p.Name.ToLower().Contains(parameters.SearchTerm.ToLower())))

        {
            #region Including
            AddInclude(p => p.Category);
            AddInclude(p => p.Supplier); 
            #endregion
            #region Sorting
            switch (parameters.sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;

                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    break;
            }
            #endregion
            #region Pagination
            ApplyPagination(parameters.PageIndex, parameters.PageSize); 
            #endregion
        }
        public ProductWithCategoryAndSupplierSpecification(int id)
            : base(p=>p.Id==id)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Supplier);
        }
    }
}
