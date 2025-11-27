using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configuration
{
    public class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
        {
           builder.Property(p => p.UnitPrice).HasColumnType("decimal(15,2)");
           builder.Property(p => p.Subtotal).HasColumnType("decimal(15,2)");
            builder.Property(p=>p.Subtotal).HasComputedColumnSql("[Quantity] * [UnitPrice]");
            builder.HasOne(p => p.Product)
                   .WithMany(p => p.PurchaseOrderDetails)
                   .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.PurchaseOrder)
                   .WithMany(p => p.PurchaseOrderDetails)
                   .HasForeignKey(p => p.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
