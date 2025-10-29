using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.UserService.DataAccessLayer.Repositories.Abstractions
{
    public interface ITransactionOutboxRepository : IBaseRepository<OutboxEvent>
    {
        Task MaskAsFailedAsync(OutboxEvent outboxEvent, bool recoverable = true);
        Task MaskAsProcessedAsync(OutboxEvent outboxEvent);
        Task<IEnumerable<OutboxEvent>> GetUnpublishedMessagesAsync(int size = 10);
        Task SaveChangesAsync();
    }
}
