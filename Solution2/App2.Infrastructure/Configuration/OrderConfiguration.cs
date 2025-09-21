using App2.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App2.Infrastructure.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key).ValueGeneratedNever().IsRequired();

            builder.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
