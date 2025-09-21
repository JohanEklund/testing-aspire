using App1.Infrastructure;
using App1.MigrationService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    var connectionString = builder.Configuration["app1:db:cs"];
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string for App1 is not configured.");
    }
    options.UseSqlServer(connectionString);
});

builder.EnrichSqlServerDbContext<AppDbContext>();

var host = builder.Build();
host.Run();
