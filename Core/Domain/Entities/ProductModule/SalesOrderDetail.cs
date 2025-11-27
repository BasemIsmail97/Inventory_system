using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class SalesOrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        #region Product navigational property
        public int ProductId { get; set; }
        public Product Product { get; set; } 
        #endregion
        #region Sales Order navigational property
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } 
        #endregion
    }
}
