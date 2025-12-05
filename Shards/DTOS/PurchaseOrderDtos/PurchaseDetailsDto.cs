using Domain.Enums;
using Shards.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.PurchaseOrderDtos
{
    public class PurchaseDetailsDto
    {
        public string InvoiceNumber { get; set; } = string.Empty; 
        public decimal TotalAmount { get; set; }
        [EnumDataType(typeof(PaymentStatus))]
        public string PaymentStatus { get; set; }
        [EnumDataType(typeof(OrderStatus))]
        public string OrderStatus { get; set; }
        public decimal RemainingAmount { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public List<PurchaseOrderDetailDto> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetailDto>();
        public DateTime CreatedAt { get; set; }
    }
}
