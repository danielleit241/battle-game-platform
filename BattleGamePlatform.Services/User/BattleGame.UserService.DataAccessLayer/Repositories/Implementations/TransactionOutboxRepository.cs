using BattleGame.Shared.Database.Repositories;

namespace BattleGame.UserService.DataAccessLayer.Repositories.Implementations
{
    public class TransactionOutboxRepository(UserDbContext context, TransactionOutboxRepositoryOptions opt) : PostgresRepository<OutboxEvent>(context), ITransactionOutboxRepository
    {
        public async Task<IEnumerable<OutboxEvent>> GetUnpublishedMessagesAsync(int size = 10)
        {
            var events = await GetAllAsync(e => e.ProcessedAt == null && e.ProcessedCount < opt.MaxRetries);
            var result = events.Take(size);
            return result;
        }

        public Task MaskAsFailedAsync(OutboxEvent outboxEvent, bool recoverable = true)
        {
            if (recoverable)
            {
                outboxEvent.ProcessedCount++;
            }
            else
            {
                outboxEvent.ProcessedCount = opt.MaxRetries;
            }

            outboxEvent.UpdatedAt = DateTime.UtcNow;
            context.OutboxEvents.Update(outboxEvent);

            return Task.CompletedTask;
        }


        public Task MaskAsProcessedAsync(OutboxEvent outboxEvent)
        {
            outboxEvent.ProcessedCount++;
            outboxEvent.ProcessedAt = DateTime.UtcNow;
            outboxEvent.UpdatedAt = DateTime.UtcNow;

            context.OutboxEvents.Update(outboxEvent);

            return Task.CompletedTask;
        }


        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
