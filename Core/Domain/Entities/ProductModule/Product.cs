using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
      
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStockLevel { get; set; }
        public string? PictureUrl { get; set; } = string.Empty;
        public Collection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new Collection<PurchaseOrderDetail>();
        public Collection<SalesOrderDetail> SalesOrderDetails { get; set; } = new Collection<SalesOrderDetail>();
        #region Category navigational property
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
        #endregion
        #region Supplier navigational property
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } 
        #endregion


    }
}
