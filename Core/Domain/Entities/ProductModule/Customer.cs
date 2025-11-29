using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class Customer : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public Collection<SalesOrder> SalesOrders { get; set; } = new Collection<SalesOrder>();


    }
}
