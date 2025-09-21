using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using App1.Infrastructure;

namespace App1.CommonDependencies
{
    public static class RegisterModules
    {
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["app1:db:cs"];

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for App1 is not configured.");
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
