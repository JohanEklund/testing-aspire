using App1.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App1.Infrastructure.DbConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key).ValueGeneratedNever().IsRequired();
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);

            builder.HasData(
                new User { Key = Guid.Parse("11111111-1111-1111-1111-111111111111"), Id = 1, FirstName = "John", LastName = "Doe" },
                new User { Key = Guid.Parse("22222222-2222-2222-2222-222222222222"), Id = 2, FirstName = "Jane", LastName = "Smith" }
            );
        }
    }
}
