using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class CreatedMatchEventHandler : IConsumer<CreatedMatchIntergrationEvent>
    {
        private readonly ILogger<CreatedMatchEventHandler> _logger;
        private readonly ITournamentMatchRepository _tournamentMatchRepository;
        public CreatedMatchEventHandler(ILogger<CreatedMatchEventHandler> logger, ITournamentMatchRepository tournamentMatchRepository)
        {
            _logger = logger;
            _tournamentMatchRepository = tournamentMatchRepository;
        }
        public async Task Consume(ConsumeContext<CreatedMatchIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("CreatedMatchEventHandler received CreatedMatchIntergrationEvent: {Message}", message);
            var match = message.ToEntity();
            if (match == null)
            {
                _logger.LogError("Failed to map CreatedMatchIntergrationEvent to Match entity: {Message}", message);
                return;
            }
            await _tournamentMatchRepository.AddAsync(match);
            _logger.LogInformation("Match entity added to repository: {Match}", match);
        }
    }
}
