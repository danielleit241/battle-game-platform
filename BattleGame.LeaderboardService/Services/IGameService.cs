using BattleGame.MessageBus.Events;

namespace BattleGame.LeaderboardService.Services
{
    public interface IGameService
    {
        public Task AddGame(GameCreatedEvent @event);
        public Task UpdateGame(GameUpdatedEvent @event);
        public Task DeleteGame(GameDeletedEvent @event);
    }
}
