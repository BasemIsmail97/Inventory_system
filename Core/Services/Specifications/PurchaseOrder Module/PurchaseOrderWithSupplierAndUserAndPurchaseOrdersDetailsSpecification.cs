

namespace Services.Specifications.PurchaseOrder_Module
{
    public class PurchaseOrderWithSupplierAndUserAndPurchaseOrdersDetailsSpecification : BaseSpecifcation<PurchaseOrder>
    {
        public PurchaseOrderWithSupplierAndUserAndPurchaseOrdersDetailsSpecification(PurchaseOrderSpecificationParameters parameters)
           : base(p => (!parameters.SupplierId.HasValue || p.SupplierId == parameters.SupplierId) && (string.IsNullOrEmpty(parameters.ApplicationUserId) || p.ApplicationUserId.ToLower().Contains(parameters.ApplicationUserId.ToLower())
                    && (string.IsNullOrEmpty(parameters.SearchTerm) || p.InvoiceNumber.ToLower().Contains(parameters.SearchTerm.ToLower()))))

        {
            #region Including
            AddInclude(p => p.Supplier);
            AddInclude(p => p.ApplicationUser);
            AddInclude(p => p.PurchaseOrderDetails);
            #endregion
            #region Pagination
            ApplyPagination(parameters.PageIndex, parameters.PageSize);
            #endregion
        }
        public PurchaseOrderWithSupplierAndUserAndPurchaseOrdersDetailsSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.Supplier);
            AddInclude(p => p.ApplicationUser);
            AddInclude(p => p.PurchaseOrderDetails);
        }
    }
}
