using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class CreatedTournamentEventHandler : IConsumer<CreatedTournamentIntergrationEvent>
    {
        private readonly ILogger<CreatedTournamentEventHandler> _logger;
        private readonly ITournamentRepository tournamentRepository;
        public CreatedTournamentEventHandler(ILogger<CreatedTournamentEventHandler> logger, ITournamentRepository tournamentRepository)
        {
            _logger = logger;
            this.tournamentRepository = tournamentRepository;
        }

        public async Task Consume(ConsumeContext<CreatedTournamentIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Received CreatedTournamentIntergrationEvent for Tournament Id: {TournamentId}", message.Id);
            var tournament = message.ToEntity();
            if (tournament == null)
            {
                _logger.LogError("Failed to convert CreatedTournamentIntergrationEvent to Tournament entity for Tournament Id: {TournamentId}", message.Id);
                return;
            }
            await tournamentRepository.AddAsync(tournament);
            _logger.LogInformation("Tournament with Id: {TournamentId} has been added to the history database.", message.Id);
        }
    }
}
