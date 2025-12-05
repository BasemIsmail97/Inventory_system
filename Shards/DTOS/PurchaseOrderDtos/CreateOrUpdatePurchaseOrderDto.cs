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
    public class CreateOrUpdatePurchaseOrderDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Invoice Number Is Required")]
        public string InvoiceNumber { get; set; } = string.Empty;
       
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
        public DateTime UpdatedAt { get; set; }
        private decimal totalAmount;

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set {
                if (PurchaseOrderDetails != null && PurchaseOrderDetails.Count > 0)
                {
                    
                    foreach (var item in PurchaseOrderDetails)
                        totalAmount += item.Subtotal;

                }
                else
                {
                    totalAmount = 0;

                }
; }
        }


    }
}
