namespace BattleGame.LeaderboardService.Consumers
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
                var message = context.Message;
                _logger.LogInformation("Received GameDeletedEvent for GameId: {GameId}", message.GameId);
                var game = new Game
                {
                    Id = message.GameId
                };
                await _gameRepository.DeleteAsync(game);
                _logger.LogInformation("Deleted Game with Id: {GameId} from Leaderboard database", message.GameId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GameDeletedEvent: {ErrorMessage}", ex.Message);
            }
        }
    }
}
