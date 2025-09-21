using App2.Infrastructure;
using App2.MigrationService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContextPool<App2DbContext>(options =>
{
    var connectionString = builder.Configuration["app2:db:cs"];
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string for App1 is not configured.");
    }
    options.UseSqlServer(connectionString);
});

builder.EnrichSqlServerDbContext<App2DbContext>();

var host = builder.Build();
host.Run();
