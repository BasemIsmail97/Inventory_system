using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.SalesOrderDtos
{
    public class SalesOrderDetailDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
    }
}
