using BattleGame.MatchService.Services;
using BattleGame.MessageBus.Events;
using MassTransit;

namespace BattleGame.MatchService.Consumers
{
    public class GameCompletedEventConsumer : IConsumer<GameCompletedEvent>
    {
        private readonly ILogger<GameCompletedEventConsumer> _logger;
        private readonly IMatchLogService _matchLogService;
        public GameCompletedEventConsumer(ILogger<GameCompletedEventConsumer> logger, IMatchLogService service)
        {
            _logger = logger;
            _matchLogService = service;
        }
        public async Task Consume(ConsumeContext<GameCompletedEvent> context)
        {
            var message = context.Message;
            _logger.LogDebug("Received GameCompletedEvent: {@GameCompletedEvent}", message);
            await _matchLogService.CreateMatch(message);
            _logger.LogDebug("Match log created for GameId: {GameId}, UserId: {UserId}", message.GameId, message.UserId);
        }
    }
}