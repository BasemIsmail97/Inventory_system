using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.SpplierDtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
