using Domain.Enums;
using Shards.DTOS.PurchaseOrderDtos;
using Shards.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.SalesOrderDtos
{
    public class CreateOrUpdateSalesOrderDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Invoice Number Is Required")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [EnumDataType(typeof(PaymentStatus))]
        public string PaymentStatus { get; set; }
        [EnumDataType(typeof(OrderStatus))]
        public string OrderStatus { get; set; }
        public decimal RemainingAmount { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public List<SalesOrderDetailDto> SalesOrderDetails { get; set; } = new List<SalesOrderDetailDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        private decimal totalAmount;

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                if (SalesOrderDetails != null && SalesOrderDetails.Count > 0)
                {

                    foreach (var item in SalesOrderDetails)
                        totalAmount += item.Subtotal;

                }
                else
                {
                    totalAmount = 0;

                }
;
            }
        }
    }
}
