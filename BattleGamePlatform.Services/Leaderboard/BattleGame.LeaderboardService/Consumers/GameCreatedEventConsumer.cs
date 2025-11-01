namespace BattleGame.LeaderboardService.Consumers
{
    public class GameCreatedEventConsumer : IConsumer<GameCreatedEvent>
    {
        private readonly ILogger<GameCreatedEventConsumer> _logger;
        private readonly IGameRepository _gameRepository;
        public GameCreatedEventConsumer(ILogger<GameCreatedEventConsumer> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }
        public async Task Consume(ConsumeContext<GameCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogDebug("Game created event received: {Message}", message);
            var game = new Game
            {
                Id = message.GameId,
                GameName = message.GameName,
                CreatedAt = message.CreatedAt
            };
            await _gameRepository.AddAsync(game);
            _logger.LogDebug("Game created event processed: {Message}", message);
        }
    }
}
