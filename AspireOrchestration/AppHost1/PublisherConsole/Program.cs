using Azure.Messaging.ServiceBus;
using PublisherConsole;
using System.Text.Json;

// Get Service Bus connection string from environment variable (set by AppHost)
var connectionString = Environment.GetEnvironmentVariable("app1:sb:cs");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Service Bus connection string not found in environment variable 'app1:sb:cs'.");
    return;
}

const string topicName = "mytopic";

// Create a ServiceBusClient and sender
await using var client = new ServiceBusClient(connectionString);
ServiceBusSender sender = client.CreateSender(topicName);

// Sample data for randomization
string[] firstNames = { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank" };
string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia" };

var random = new Random();

// Randomize fields
int userId = random.Next(1, 10000);
string firstName = firstNames[random.Next(firstNames.Length)];
string lastName = lastNames[random.Next(lastNames.Length)];
string orderNumber = $"ORD-{random.Next(1000, 9999)}";

// Create a UserOrderCreated message
var userOrderCreated = new UserOrderCreated
{
    UserId = userId,
    FirstName = firstName,
    LastName = lastName,
    OrderNumber = orderNumber,
    OrderDate = DateTimeOffset.UtcNow
};

// Serialize the message to JSON
string messageBody = JsonSerializer.Serialize(userOrderCreated);
var message = new ServiceBusMessage(messageBody)
{
    Subject = "order.created" // This matches the CorrelationFilter in AppHost.cs
};

// Send the message
await sender.SendMessageAsync(message);

Console.WriteLine($"UserOrderCreated message published to topic: UserId={userId}, Name={firstName} {lastName}, OrderNumber={orderNumber}.");