using Domain.Entities.IdentityModule;
using Domain.Enums;
using Shards.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class SalesOrder : BaseEntity<int>
    {
        
        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal RemainingAmount { get; set; }
        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }    = new List<SalesOrderDetail>();
        #region Customer navigational property
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        #endregion
        #region User navigational property
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } 
        #endregion
    }
}
