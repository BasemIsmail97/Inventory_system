using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
           builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
           builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnType("decimal(15,2)");
            builder.HasOne(p=>p.Category).WithMany(c=>c.Products).HasForeignKey(p => p.CategoryId) .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p=>p.Supplier).WithMany(c=>c.products).HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);
            


        }
    }
}
