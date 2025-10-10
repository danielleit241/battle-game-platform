using BattleGame.MatchService.Services;
using BattleGame.MessageBus.Events;
using MassTransit;

namespace BattleGame.MatchService.Consumers
{
    public class GameCompletedConsumer : IConsumer<GameCompletedEvent>
    {
        private readonly ILogger<GameCompletedConsumer> _logger;
        private readonly IMatchLogService _matchLogService;
        public GameCompletedConsumer(ILogger<GameCompletedConsumer> logger, IMatchLogService service)
        {
            _logger = logger;
            _matchLogService = service;
        }
        public async Task Consume(ConsumeContext<GameCompletedEvent> context)
        {
            var message = context.Message;
            _logger.LogDebug("Received GameCompletedEvent: {@GameCompletedEvent}", message);
            var gameCompletedEvent = new GameCompletedEvent
            (
                GameId: message.GameId,
                UserId: message.UserId,
                CompletedAt: message.CompletedAt
            );
            _logger.LogDebug("GameCompletedEvent consumed: {@GameCompletedEvent}", gameCompletedEvent);
            await _matchLogService.CreateMatch(gameCompletedEvent);
            _logger.LogDebug("Match log created for GameId: {GameId}, UserId: {UserId}", message.GameId, message.UserId);
        }
    }
}