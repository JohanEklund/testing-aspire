using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App2.Infrastructure.Configuration
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<App2DbContext>
    {
        public App2DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<App2DbContext>();

            optionsBuilder.UseSqlServer("not empty");

            return new App2DbContext(optionsBuilder.Options);
        }
    }
}
