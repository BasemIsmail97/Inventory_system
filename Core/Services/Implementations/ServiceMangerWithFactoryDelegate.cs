using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceMangerWithFactoryDelegate (Func<IProductService> _productFactory, Func<IPurchaseOrderService> _purchaseOrderFactory, Func<ICategoryServicecs> _categoryFactory, Func<ICustomerService> _customerFactory,
        Func<ISupplierServicecs> _supplierFactory, Func<ISalesOrderService> _SalesOrderFactory) : IServiceManger
    {
        public IPurchaseOrderService PurchaseOrderService =>_purchaseOrderFactory.Invoke();

        public ISalesOrderService SalesOrderService => _SalesOrderFactory.Invoke();

        public IProductService ProductService =>_productFactory.Invoke();

        public ICustomerService CustomerService =>_customerFactory.Invoke();

        public ICategoryServicecs CategoryService => _categoryFactory.Invoke();

        public ISupplierServicecs SupplierService =>_supplierFactory.Invoke();
    }
}
