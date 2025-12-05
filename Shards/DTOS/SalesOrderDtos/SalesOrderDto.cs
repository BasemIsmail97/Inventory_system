using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.SalesOrderDtos
{
    public class SalesOrderDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
    }
}
