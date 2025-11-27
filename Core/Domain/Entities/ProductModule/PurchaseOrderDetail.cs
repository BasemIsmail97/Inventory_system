using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal{ get; set; }
        #region Prodcut navigational property
        public int ProductId { get; set; }
        public Product Product { get; set; } 
        #endregion

        #region Purchase order navigational property
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } 
        #endregion


    }
}
