

namespace Services.Specifications.PurchaseOrder_Module
{
    public class PurchaseOrderCountSpecification : BaseSpecifcation<PurchaseOrder>
    {
        public PurchaseOrderCountSpecification(PurchaseOrderSpecificationParameters parameters)
          : base(p => (!parameters.SupplierId.HasValue || p.SupplierId == parameters.SupplierId) && (string.IsNullOrEmpty(parameters.ApplicationUserId) || p.ApplicationUserId.ToLower().Contains(parameters.ApplicationUserId.ToLower())
                    && (string.IsNullOrEmpty(parameters.SearchTerm) || p.InvoiceNumber.ToLower().Contains(parameters.SearchTerm.ToLower()))))

        {

        }

    }
}
