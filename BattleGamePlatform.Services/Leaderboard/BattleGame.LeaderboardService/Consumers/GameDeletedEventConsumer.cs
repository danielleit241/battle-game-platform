using BattleGame.LeaderboardService.Repositories;
using BattleGame.MessageBus.Events;
using MassTransit;

namespace BattleGame.MatchService.Consumers
{
    public class GameDeletedEventConsumer : IConsumer<GameDeletedEvent>
    {
        private readonly ILogger<GameDeletedEventConsumer> _logger;
        private readonly IGameRepository _gameRepository;
        public GameDeletedEventConsumer(ILogger<GameDeletedEventConsumer> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }
        public async Task Consume(ConsumeContext<GameDeletedEvent> context)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
