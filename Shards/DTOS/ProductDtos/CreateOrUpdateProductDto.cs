using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.ProductDtos
{
    public class CreateOrUpdateProductDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Product Name Is Required")]
        public string Name { get; set; } = string.Empty;
        [Range(1, double.MaxValue, ErrorMessage = "Price must be more than 0.")]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStockLevel { get; set; }
        public string? PictureUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
