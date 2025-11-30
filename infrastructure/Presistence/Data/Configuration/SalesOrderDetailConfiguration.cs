
namespace Persistence.Data.Configuration
{
    public class SalesOrderDetailConfiguration : IEntityTypeConfiguration<SalesOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SalesOrderDetail> builder)
        {
            builder.Property(p => p.UnitPrice).HasColumnType("decimal(15,2)");
            builder.Property(p => p.Subtotal).HasColumnType("decimal(15,2)");
            builder.Property(p => p.Subtotal).HasComputedColumnSql("[Quantity] * [UnitPrice]");
            builder.HasOne(p => p.Product)
                   .WithMany(p => p.SalesOrderDetails)
                   .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.SalesOrder)
                   .WithMany(p => p.SalesOrderDetails)
                   .HasForeignKey(p => p.SalesOrderId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
