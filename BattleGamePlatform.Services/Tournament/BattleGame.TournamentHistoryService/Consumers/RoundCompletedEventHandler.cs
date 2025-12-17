using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class RoundCompletedEventHandler : IConsumer<RoundCompletedIntergrationEvent>
    {
        private readonly ILogger<RoundCompletedEventHandler> _logger;
        private readonly ITournamentRoundRepository _tournamentRoundRepository;

        public RoundCompletedEventHandler(
            ILogger<RoundCompletedEventHandler> logger, 
            ITournamentRoundRepository tournamentRoundRepository)
        {
            _logger = logger;
            _tournamentRoundRepository = tournamentRoundRepository;
        }

        public async Task Consume(ConsumeContext<RoundCompletedIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("RoundCompletedEventHandler received event for Round {RoundId}, Tournament {TournamentId}", 
                message.RoundId, message.TournamentId);

            var round = await _tournamentRoundRepository.GetAsync(message.RoundId);
            if (round == null)
            {
                _logger.LogError("Round {RoundId} not found in history database", message.RoundId);
                return;
            }

            round.Status = Entities.TournamentRoundStatus.Completed;
            round.UpdatedAt = message.CompletedAt;
            await _tournamentRoundRepository.UpdateAsync(round);

            _logger.LogInformation("Round {RoundId} (Round Number: {RoundNumber}) marked as completed", 
                message.RoundId, message.RoundNumber);
        }
    }
}
