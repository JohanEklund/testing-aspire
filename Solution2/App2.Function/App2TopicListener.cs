using App2.Infrastructure;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App2.Function
{
    public class App2TopicListener
    {
        private readonly App2DbContext dbContext;

        public App2TopicListener(App2DbContext dbContext, ILogger<App2TopicListener> logger)
        {
            this.dbContext = dbContext;
            logger.LogInformation("App2TopicListenerStarted!");
        }

        [Function(nameof(App2TopicListener))]
        public async Task Run(
            [ServiceBusTrigger("mytopic", "app2subscription", Connection = "app2:sb:cs")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {

            var orderCreated = message.Body.ToObjectFromJson<OrderCreated>();

            if (orderCreated is null)
            {
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Failed to deserialize message body to OrderCreated.");
                return;
            }

            var existing = await dbContext.Orders.SingleOrDefaultAsync(x => x.OrderNumber == orderCreated.OrderNumber);

            if (existing is not null)
            {
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Order already exists.");
                return;
            }

            var order = new Domain.Order
            {
                Key = Guid.NewGuid(),
                UserId = orderCreated.UserId,
                OrderNumber = orderCreated.OrderNumber,
                OrderDate = orderCreated.OrderDate
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
