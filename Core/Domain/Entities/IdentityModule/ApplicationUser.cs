using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IdentityModule
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public Collection<SalesOrder> SalesOrders { get; set; } = new Collection<SalesOrder>();
        public Collection<PurchaseOrder> purchaseOrders { get; set; } = new Collection<PurchaseOrder>();

    }
}
