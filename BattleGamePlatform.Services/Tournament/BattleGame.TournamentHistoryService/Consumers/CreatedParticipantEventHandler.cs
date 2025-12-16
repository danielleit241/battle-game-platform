using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGame.TournamentService.IntergrationEvents;
using MassTransit;

namespace BattleGame.TournamentHistoryService.Consumers
{
    public class CreatedParticipantEventHandler : IConsumer<CreatedParticipantIntergrationEvent>
    {
        private readonly ILogger<CreatedParticipantEventHandler> _logger;
        private readonly ITournamentParticipantRepository tournamentParticipantRepository;
        public CreatedParticipantEventHandler(ILogger<CreatedParticipantEventHandler> logger, ITournamentParticipantRepository tournamentParticipantRepository)
        {
            _logger = logger;
            this.tournamentParticipantRepository = tournamentParticipantRepository;
        }
        public async Task Consume(ConsumeContext<CreatedParticipantIntergrationEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Received CreatedParticipantIntergrationEvent: {Message}", message);
            var participant = message.ToEntity();
            if (participant == null)
            {
                _logger.LogError("Failed to convert CreatedParticipantIntergrationEvent to Participant entity for Participant Id: {ParticipantId}", message.Id);
                return;
            }
            await tournamentParticipantRepository.AddAsync(participant);
            _logger.LogInformation("Participant with Id: {ParticipantId} has been added to the history database.", message.Id);
        }
    }
}
