using App1.Domain;
using Microsoft.EntityFrameworkCore;

namespace App1.Infrastructure
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
