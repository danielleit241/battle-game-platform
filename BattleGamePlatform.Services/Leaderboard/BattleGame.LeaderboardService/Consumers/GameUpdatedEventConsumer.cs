namespace BattleGame.LeaderboardService.Consumers
{
    public class GameUpdatedEventConsumer : IConsumer<GameUpdatedEvent>
    {
        private readonly ILogger<GameUpdatedEventConsumer> _logger;
        private readonly IGameRepository _gameRepository;

        public GameUpdatedEventConsumer(ILogger<GameUpdatedEventConsumer> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }
        public async Task Consume(ConsumeContext<GameUpdatedEvent> context)
        {
            try
            {
                var gameEvent = context.Message;
                _logger.LogInformation("Received GameUpdatedEvent for GameId: {GameId}", gameEvent.GameId);
                var game = new Game
                {
                    Id = gameEvent.GameId,
                    GameName = gameEvent.GameName,
                    UpdatedAt = gameEvent.UpdatedAt
                };
                await _gameRepository.UpdateAsync(game);
                _logger.LogInformation("Game with GameId: {GameId} updated successfully.", gameEvent.GameId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GameUpdatedEvent for GameId: {GameId}", context.Message.GameId);
            }
        }
    }
}
