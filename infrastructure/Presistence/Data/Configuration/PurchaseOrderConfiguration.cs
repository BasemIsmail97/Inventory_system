

namespace Presistence.Data.Configuration
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.Property(p => p.TotalAmount).HasColumnType("decimal(15,2)");
            builder.Property(p => p.RemainingAmount).HasColumnType("decimal(15,2)");
            builder.HasOne(p=>p.Supplier).WithMany(s => s.PurchaseOrders)
                   .HasForeignKey(p => p.SupplierId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
            builder.HasOne(p => p.ApplicationUser).WithMany(a => a.purchaseOrders)
                   .HasForeignKey(p => p.ApplicationUserId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        }
    }
}
