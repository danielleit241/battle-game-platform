using BattleGame.LeaderboardService.Repositories;
using BattleGame.MessageBus.Events;

namespace BattleGame.LeaderboardService.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        }
        public async Task AddGame(GameCreatedEvent @event)
        {
            var game = @event.AsEntity();
            await gameRepository.AddAsync(game);
        }

        public async Task DeleteGame(GameDeletedEvent @event)
        {
            var game = await gameRepository.GetAsync(game => game.Id == @event.GameId);
            if (game != null)
            {
                await gameRepository.DeleteAsync(game);
            }
        }

        public async Task UpdateGame(GameUpdatedEvent @event)
        {
            var game = await gameRepository.GetAsync(game => game.Id == @event.GameId);
            if (game != null)
            {
                game = @event.AsEntity();
                await gameRepository.UpdateAsync(game);
            }
        }
    }
}
