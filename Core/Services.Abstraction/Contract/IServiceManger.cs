using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface IServiceManger
    {
        IPurchaseOrderService PurchaseOrderService { get; }
        ISalesOrderService SalesOrderService { get; }
        IProductService ProductService { get; }

        ICustomerService CustomerService { get; }
        ICategoryServicecs CategoryService { get; }
        ISupplierServicecs SupplierService { get; }
        IAuthService AuthService { get; }
    }
}
