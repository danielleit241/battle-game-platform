using BattleGame.LeaderboardService.Services;
using BattleGame.MessageBus.Events;
using MassTransit;

namespace BattleGame.MatchService.Consumers
{
    public class GameCreatedEventConsumer : IConsumer<GameCreatedEvent>
    {
        private readonly ILogger<GameCreatedEventConsumer> _logger;
        private readonly IGameService _gameService;
        public GameCreatedEventConsumer(ILogger<GameCreatedEventConsumer> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }
        public async Task Consume(ConsumeContext<GameCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogDebug("Game created event received: {Message}", message);
            await _gameService.AddGame(message);
            _logger.LogDebug("Game created event processed: {Message}", message);
        }
    }
}
