using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;
using MassTransit;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Tournament.Command
{
    public record CreateTournamentCommand(CreateTournamentDto Dto) : IRequest<ApiResponse<TournamentDto>>;
    public class CreateTournamentHandler : IRequestHandler<CreateTournamentCommand, ApiResponse<TournamentDto>>
    {
        private readonly ILogger<CreateTournamentHandler> _logger;
        private readonly ITournamentWriteRepository _tournamentWriteRepository;
        private readonly ITournamentRoundWriteRepository _tournamentRoundWriteRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateTournamentHandler(ILogger<CreateTournamentHandler> logger,
            ITournamentWriteRepository tournamentWriteRepository,
            ITournamentRoundWriteRepository tournamentRoundWriteRepository,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _tournamentWriteRepository = tournamentWriteRepository;
            _tournamentRoundWriteRepository = tournamentRoundWriteRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ApiResponse<TournamentDto>> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateTournamentCommand");

            var tournamentEntity = request.Dto.AsTournamentEntity();
            _logger.LogInformation("Mapped CreateTournamentDto to Tournament entity");

            if (!IsPowerOfTwo(tournamentEntity.MaxParticipants))
            {
                _logger.LogWarning("MaxParticipants {MaxParticipants} is not a power of two", tournamentEntity.MaxParticipants);
                return ApiResponse<TournamentDto>.FailureResponse("MaxParticipants must be a power of two (e.g., 2, 4, 8, 16, 32, etc.)");
            }

            await _tournamentWriteRepository.AddAsync(tournamentEntity);
            _logger.LogInformation("Tournament entity added to repository");

            await _publishEndpoint.Publish(tournamentEntity.AsTournamentCreatedEvent(), cancellationToken);
            _logger.LogInformation("Published TournamentCreatedEvent");

            var rounds = await _tournamentRoundWriteRepository.GenerateRoundsForTournamentAsync(tournamentEntity.Id, tournamentEntity.MaxParticipants);

            _logger.LogInformation("Generated tournament rounds for Tournament ID: {TournamentId}", tournamentEntity.Id);

            foreach (var item in rounds)
            {
                _logger.LogInformation("Publishing RoundCreatedEvent for Round ID: {RoundId}", item.Id);
                await _publishEndpoint.Publish(item.AsRoundCreatedEvent(), cancellationToken);
            }

            var tournamentDto = tournamentEntity.AsTournamentDto();
            _logger.LogInformation("Mapped Tournament entity to TournamentDto");

            return ApiResponse<TournamentDto>.SuccessResponse(tournamentDto, "Tournament created successfully");
        }

        private static bool IsPowerOfTwo(int number)
        {
            return (number > 0) && ((number & (number - 1)) == 0);
        }
    }
}
