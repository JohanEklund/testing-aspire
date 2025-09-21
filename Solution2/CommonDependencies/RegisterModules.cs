using App2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonDependencies
{
    public static class RegisterModules
    {
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["app2:db:cs"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for App2 is not configured.");
            }

            services.AddDbContext<App2DbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddHttpClient<IApp1Client, App1Client>(client =>
            {
                client.BaseAddress = new Uri(configuration["app1:api:url"] ?? throw new InvalidOperationException("App1 API URL is not configured."));
            });
        }
    }
}
