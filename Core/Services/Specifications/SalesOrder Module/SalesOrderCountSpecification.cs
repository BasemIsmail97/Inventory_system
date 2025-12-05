using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.SalesOrder_Module
{
    public class SalesOrderCountSpecification : BaseSpecifcation<Domain.Entities.ProductModule.SalesOrder>
    {
        public SalesOrderCountSpecification(SalesOrderSpecificationParameters parameters)
          : base(so => (!parameters.CustomerId.HasValue || so.CustomerId == parameters.CustomerId) && (string.IsNullOrEmpty(parameters.ApplicationUserId) || so.ApplicationUserId.ToLower().Contains(parameters.ApplicationUserId.ToLower())
                    && (string.IsNullOrEmpty(parameters.SearchTerm) || so.InvoiceNumber.ToLower().Contains(parameters.SearchTerm.ToLower()))))
        {
        }

    }
}
