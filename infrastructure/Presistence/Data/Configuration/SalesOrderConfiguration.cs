

namespace Presistence.Data.Configuration
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.Property(p => p.TotalAmount).HasColumnType("decimal(15,2)");
            builder.Property(p => p.RemainingAmount).HasColumnType("decimal(15,2)");
            builder.HasOne(p => p.Customer).WithMany(c => c.SalesOrders).HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.ApplicationUser).WithMany(a => a.SalesOrders).HasForeignKey(p => p.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
