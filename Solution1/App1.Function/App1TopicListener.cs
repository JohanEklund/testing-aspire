using App1.Infrastructure;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App1.Function
{
    public class App1TopicListener
    {
        private readonly AppDbContext dbContext;

        public App1TopicListener(AppDbContext dbContext, ILogger<App1TopicListener> logger)
        {
            this.dbContext = dbContext;
            logger.LogInformation("App1TopicListenerStarted!");
        }

        [Function(nameof(App1TopicListener))]
        public async Task Run(
            [ServiceBusTrigger("mytopic", "app1subscription", Connection = "app1:sb:cs")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            var userCreated = message.Body.ToObjectFromJson<UserCreated>();

            if (userCreated is null)
            {
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Message body could not be deserialized to UserCreated.");
                return;
            }

            var existing = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userCreated.UserId);

            if (existing == null)
            {
                await dbContext.Users.AddAsync(new Domain.User
                {
                    Key = Guid.NewGuid(),
                    Id = userCreated.UserId,
                    FirstName = userCreated.FirstName,
                    LastName = userCreated.LastName
                });
            }
            else
            {
                existing.FirstName = userCreated.FirstName;
                existing.LastName = userCreated.LastName;
            }

            await dbContext.SaveChangesAsync();

            await messageActions.CompleteMessageAsync(message);
        }
    }
}
