using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.SalesOrder_Module
{
    public class SalesOrderWithCustomerAndUserAndSalesOrdersDetailsSpecification : BaseSpecifcation<SalesOrder>
    {
        public SalesOrderWithCustomerAndUserAndSalesOrdersDetailsSpecification(SalesOrderSpecificationParameters parameters)
            :  base(p => (!parameters.CustomerId.HasValue || p.CustomerId == parameters.CustomerId) && (string.IsNullOrEmpty(parameters.ApplicationUserId) || p.ApplicationUserId.ToLower().Contains(parameters.ApplicationUserId.ToLower())
                    && (string.IsNullOrEmpty(parameters.SearchTerm) || p.InvoiceNumber.ToLower().Contains(parameters.SearchTerm.ToLower()))))
        {
            #region Including
            AddInclude(so => so.Customer);
            AddInclude(so => so.ApplicationUser);
            AddInclude(so => so.SalesOrderDetails);
            #endregion



            #region Pagination
            ApplyPagination(parameters.PageIndex, parameters.PageSize); 
            #endregion
        }





        public SalesOrderWithCustomerAndUserAndSalesOrdersDetailsSpecification(int salesOrderId)
            : base(so => so.Id == salesOrderId)
        {
            AddInclude(so => so.Customer);
            AddInclude(so => so.ApplicationUser);
            AddInclude(so => so.SalesOrderDetails);
        }
    }
}
