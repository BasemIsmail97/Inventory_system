

namespace Persistence.Data.Configuration
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.HasKey(so => so.Id);

            builder.Property(so => so.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(so => so.OrderDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(so => so.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(so => so.RemainingAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Foreign Keys
            builder.HasOne(so => so.Customer)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(so => so.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(so => so.ApplicationUser)
                .WithMany(u => u.SalesOrders)
                .HasForeignKey(so => so.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(so => so.InvoiceNumber).IsUnique();
            builder.HasIndex(so => so.OrderDate);
            builder.HasIndex(so => so.CustomerId);
        }
    }
}
