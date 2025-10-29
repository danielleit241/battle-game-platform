using BattleGame.UserService.Common.Entities;
using BattleGame.UserService.DataAccessLayer.Repositories.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BattleGame.TransactionOutbox.Polling.Publisher
{
    public class PollingPublisher(
        ILogger<PollingPublisher> logger,
        IPublishEndpoint publishEndpoint,
        ITransactionOutboxRepository transactionOutboxRepository)
    {
        public async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            //using var scope = serviceScopeFactory.CreateScope();
            //var transactionOutboxRepository = scope.ServiceProvider.GetRequiredService<ITransactionOutboxRepository>();
            //var publishedEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();


            while (!cancellationToken.IsCancellationRequested)
            {
                var outboxMessages = await transactionOutboxRepository.GetUnpublishedMessagesAsync();

                foreach (var message in outboxMessages)
                {
                    try
                    {
                        var @event = DeserializeEvent(message);

                        if (@event == null)
                        {
                            logger.LogWarning("Skipping outbox message with ID: {MessageId} due to unknown event type", message.Id);
                            await transactionOutboxRepository.MaskAsFailedAsync(message, false);
                            continue;
                        }

                        logger.LogInformation("Publish event from Polling publisher: {m}", message.Payload);

                        await publishEndpoint.Publish(@event, cancellationToken);
                        await transactionOutboxRepository.MaskAsProcessedAsync(message);
                        await transactionOutboxRepository.SaveChangesAsync();

                        logger.LogInformation("Successfully published outbox message with ID: {MessageId}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish outbox message with ID: {MessageId}", message.Id);
                        await transactionOutboxRepository.MaskAsFailedAsync(message, true);
                    }
                }

                if (!outboxMessages.Any() && !cancellationToken.IsCancellationRequested)
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        private object? DeserializeEvent(OutboxEvent message)
        {
            var type = Type.GetType(message.Type, throwOnError: false);
            if (type == null)
            {
                logger.LogWarning("Unknown event type: {Type}", message.Type);
                return null;
            }

            return JsonSerializer.Deserialize(message.Payload, type);
        }

    }
}
