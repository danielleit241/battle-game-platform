using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class MatchCompletedEventHandler : IConsumer<MatchCompletedIntergrationEvent>
    {
        private readonly ILogger<MatchCompletedEventHandler> _logger;
        private readonly ITournamentMatchRepository _tournamentMatchRepository;

        public MatchCompletedEventHandler(
            ILogger<MatchCompletedEventHandler> logger, 
            ITournamentMatchRepository tournamentMatchRepository)
        {
            _logger = logger;
            _tournamentMatchRepository = tournamentMatchRepository;
        }

        public async Task Consume(ConsumeContext<MatchCompletedIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("MatchCompletedEventHandler received event for Match {MatchId}, Winner: {WinnerId}", 
                message.MatchId, message.WinnerId);

            var match = await _tournamentMatchRepository.GetAsync(message.MatchId);
            if (match == null)
            {
                _logger.LogError("Match {MatchId} not found in history database", message.MatchId);
                return;
            }

            match.WinnerId = message.WinnerId;
            match.UpdatedAt = message.CompletedAt;
            await _tournamentMatchRepository.UpdateAsync(match);

            _logger.LogInformation("Match {MatchId} updated with winner {WinnerId}", message.MatchId, message.WinnerId);
        }
    }
}
