using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.ProductDtos
{
    public class CreateOrUpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStockLevel { get; set; }
        public string? PictureUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
