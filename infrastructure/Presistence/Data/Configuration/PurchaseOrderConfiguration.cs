

namespace Persistence.Data.Configuration
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.HasKey(po => po.Id);

            builder.Property(po => po.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(po => po.OrderDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(po => po.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(po => po.RemainingAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(po => po.PaymentStatus)
                .IsRequired();

            builder.Property(po => po.OrderStatus)
                .IsRequired();

            // Foreign Keys
            builder.HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(po => po.ApplicationUser)
                .WithMany(u => u.purchaseOrders)
                .HasForeignKey(po => po.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(po => po.InvoiceNumber).IsUnique();
            builder.HasIndex(po => po.OrderDate);
            builder.HasIndex(po => po.SupplierId);
        }
    }
}
