using App2.Domain;
using Microsoft.EntityFrameworkCore;

namespace App2.Infrastructure
{
    public class App2DbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(App2DbContext).Assembly);
        }
    }
}
