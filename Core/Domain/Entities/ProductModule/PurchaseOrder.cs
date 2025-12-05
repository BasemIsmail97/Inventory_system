using Domain.Entities.IdentityModule;
using Domain.Enums;
using Shards.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class PurchaseOrder : BaseEntity<int>
    {
        
        public string InvoiceNumber { get; set; } = string.Empty;      
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal RemainingAmount { get; set; }
        public Collection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new Collection<PurchaseOrderDetail>();
       
        #region Supplaier navigational property
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        #endregion
        #region User navigational property
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        #endregion
    }
}
