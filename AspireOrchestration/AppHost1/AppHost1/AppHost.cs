using Azure.Provisioning.Storage;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dbServer = builder
    .AddSqlServer("database").WithLifetime(ContainerLifetime.Persistent);

var app1Db = dbServer.AddDatabase("app1db");
var app2Db = dbServer.AddDatabase("app2db");

var storage = builder.AddAzureStorage("storage").RunAsEmulator().ConfigureInfrastructure((infrastructure) =>
{
    var storageAccount = infrastructure.GetProvisionableResources().OfType<StorageAccount>().FirstOrDefault(r => r.BicepIdentifier == "storage")
        ?? throw new InvalidOperationException($"Could not find configured storage account with name 'storage'");

    storageAccount.AllowBlobPublicAccess = false;
});

var serviceBus = builder.AddAzureServiceBus("sb").RunAsEmulator(x => x.WithLifetime(ContainerLifetime.Persistent));

var topic = serviceBus.AddServiceBusTopic("mytopic");
var subscription1 = topic.AddServiceBusSubscription("app1subscription").WithProperties(x =>
{
    x.MaxDeliveryCount = 2;
    x.Rules.Clear();
    x.Rules.Add(new ("app1Rule")
    {
        CorrelationFilter = new()
        {
            Subject = "order.created"
        }
    });
});

var subscription2 = topic.AddServiceBusSubscription("app2subscription").WithProperties(x =>
{
    x.MaxDeliveryCount = 2;
    x.Rules.Clear();
    x.Rules.Add(new("app2Rule")
    {
        CorrelationFilter = new()
        {
            Subject = "order.created"
        }
    });
});

var app1MigrationService = builder.AddProject<App1_MigrationService>("app1Mig")
    .WithReference(app1Db)
    .WaitFor(app1Db)
    .WithEnvironment("app1:db:cs", app1Db.Resource.ConnectionStringExpression);

var app2MigrationService = builder.AddProject<App2_MigrationService>("app2Mig")
    .WithReference(app2Db)
    .WaitFor(app2Db)
    .WithEnvironment("app2:db:cs", app2Db.Resource.ConnectionStringExpression);

var app1Api = builder.AddProject<App1_API>("app1-api")
    .WithReference(app1Db)
    .WaitFor(app1Db)
    .WithEnvironment("app1:db:cs", app1Db.Resource.ConnectionStringExpression)
    .WaitForCompletion(app1MigrationService);

var app1Func = builder.AddAzureFunctionsProject<App1_Function>("app1-func")
    .WithReference(app1Db)
    .WaitFor(app1Db)
    .WithEnvironment("app1:db:cs", app1Db.Resource.ConnectionStringExpression)
    .WithReference(serviceBus, "app1:sb:cs")
    .WaitForCompletion(app1MigrationService);

var app2 = builder.AddProject<App2_API>("app2-api")
    .WithReference(app2Db)
    .WaitFor(app2Db)
    .WithReference(app1Api).WithEnvironment("app1:api:url", app1Api.GetEndpoint("https"))
    .WithEnvironment("app2:db:cs", app2Db.Resource.ConnectionStringExpression)    
    .WaitForCompletion(app2MigrationService);

var app2Func = builder.AddAzureFunctionsProject<App2_Function>("app2-func")
    .WithReference(app2Db)
    .WaitFor(app2Db)
    .WithReference(app1Api).WithEnvironment("app1:api:url", app1Api.GetEndpoint("https"))
    .WithEnvironment("app2:db:cs", app2Db.Resource.ConnectionStringExpression)
    .WithReference(serviceBus, "app2:sb:cs")
    .WithEnvironment("FUNCTIONS_WORKER_PORT", "7072")
    .WaitForCompletion(app2MigrationService);

var publisherConsole = builder.AddProject<PublisherConsole>("PublisherConsole")
    .WithReference(serviceBus)
    .WithEnvironment("app1:sb:cs", serviceBus.Resource.ConnectionStringExpression)
    .WaitFor(topic);

builder.Build().Run();
