using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class CreatedRoundEventHandler : IConsumer<CreatedRoundIntergrationEvent>
    {
        private readonly ILogger<CreatedRoundEventHandler> _logger;
        private readonly ITournamentRoundRepository tournamentRoundRepository;
        public CreatedRoundEventHandler(ILogger<CreatedRoundEventHandler> logger, ITournamentRoundRepository tournamentRoundRepository)
        {
            _logger = logger;
            this.tournamentRoundRepository = tournamentRoundRepository;
        }
        public async Task Consume(ConsumeContext<CreatedRoundIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("CreatedRoundEventHandler received CreatedRoundIntergrationEvent: {Message}", message);
            var tournamentRound = message.ToEntity();

            if (tournamentRound == null)
            {
                _logger.LogError("Failed to map CreatedRoundIntergrationEvent to TournamentRound entity: {Message}", message);
                return;
            }
            await tournamentRoundRepository.AddAsync(tournamentRound);
            _logger.LogInformation("TournamentRound entity added to repository: {TournamentRound}", tournamentRound);
        }
    }
}
