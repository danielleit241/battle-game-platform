namespace BattleGame.LeaderboardService.Consumers
{
    public class MatchCompletedEventConsumer : IConsumer<MatchCompletedEvent>
    {
        private readonly ILogger<MatchCompletedEventConsumer> _logger;
        private readonly ILeaderboardServices _leaderboardServices;
        public MatchCompletedEventConsumer(ILogger<MatchCompletedEventConsumer> logger, ILeaderboardServices leaderboardServices)
        {
            _logger = logger;
            _leaderboardServices = leaderboardServices;
        }
        public async Task Consume(ConsumeContext<MatchCompletedEvent> context)
        {
            var message = context.Message;
            _logger.LogDebug("Received MatchCompletedEvent: {@MatchCompletedEvent}", message);
            await _leaderboardServices.UpSertLeaderboard(message);
            _logger.LogDebug("Processed MatchCompletedEvent for MatchId: {MatchId}", message.MatchId);
        }
    }
}
