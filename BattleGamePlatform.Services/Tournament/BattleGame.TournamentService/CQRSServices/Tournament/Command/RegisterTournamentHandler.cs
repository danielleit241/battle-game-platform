using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;
using MassTransit;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Tournament.Command
{
    public record RegisterTournamentCommand(RegisterTournamentDto Dto) : IRequest<ApiResponse<TournamentDto>>;
    public class RegisterTournamentHandler : IRequestHandler<RegisterTournamentCommand, ApiResponse<TournamentDto>>
    {
        private readonly ILogger<RegisterTournamentHandler> _logger;
        private readonly ITournamentWriteRepository _tournament;
        private readonly ITournamentRoundWriteRepository _round;
        private readonly ITournamentMatchWriteRepository _match;
        private readonly ITournamentParticipantWriteRepository _participant;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Queue<RegisterTournamentDto> _tournamentQueue = new();

        public RegisterTournamentHandler(
            ILogger<RegisterTournamentHandler> logger,
            ITournamentWriteRepository tournamentWriteRepository,
            ITournamentRoundWriteRepository tournamentRoundWriteRepository,
            ITournamentMatchWriteRepository tournamentMatchWriteRepository,
            ITournamentParticipantWriteRepository tournamentParticipantWriteRepository,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _tournament = tournamentWriteRepository;
            _round = tournamentRoundWriteRepository;
            _match = tournamentMatchWriteRepository;
            _participant = tournamentParticipantWriteRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ApiResponse<TournamentDto>> Handle(RegisterTournamentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterTournamentCommand for Tournament ID: {TournamentId}", request.Dto.TournamentId);
            var tournament = await _tournament.GetAsync(request.Dto.TournamentId);
            if (tournament != null)
            {
                _logger.LogInformation("Tournament with ID: {TournamentId} already exists.", request.Dto.TournamentId);
                return ApiResponse<TournamentDto>.FailureResponse("Tournament with the same ID already exists.");
            }
            _logger.LogInformation("Fetching round information for Tournament ID: {TournamentId}", request.Dto.TournamentId);
            var round = await _round.GetRoundIsNotCompletedMatchesByTournamentId(request.Dto.TournamentId);
            if (round == null)
            {
                _logger.LogInformation("No active round found for Tournament ID: {TournamentId}", request.Dto.TournamentId);
                return ApiResponse<TournamentDto>.FailureResponse("Tournament has already been completed.");
            }
            _logger.LogInformation("Checking participant count for Tournament ID: {TournamentId}", request.Dto.TournamentId);
            bool isEnoughParticipants = await _participant.IsEnoughParticipantInTournament(tournament!.Id, tournament.MaxParticipants);
            if (!isEnoughParticipants)
            {
                _logger.LogInformation("Not enough participants to start the tournament with ID: {TournamentId}", request.Dto.TournamentId);
                return ApiResponse<TournamentDto>.FailureResponse("Not enough participants to start the tournament.");
            }

            _logger.LogInformation("Registering participant for Tournament ID: {TournamentId}", request.Dto.TournamentId);
            var participantEntity = request.Dto.AsParticipantEntity();
            await _participant.AddAsync(participantEntity);
            _logger.LogInformation("Publishing ParticipantCreatedEvent for Participant ID: {ParticipantId}", participantEntity.Id);
            await _publishEndpoint.Publish(participantEntity.AsParticipantCreatedEvent());
            _logger.LogInformation("Enqueuing participant for match pairing in Tournament ID: {TournamentId}", request.Dto.TournamentId);
            _tournamentQueue.Enqueue(request.Dto);

            if (_tournamentQueue.Count == 2)
            {
                _logger.LogInformation("Creating match for Tournament ID: {TournamentId}", request.Dto.TournamentId);
                var player1 = _tournamentQueue.Dequeue();
                var player2 = _tournamentQueue.Dequeue();
                var match = new TournamentMatch
                {
                    Id = Guid.NewGuid(),
                    RoundId = round.Id,
                    Player1Id = player1.Id,
                    Player2Id = player2.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _match.AddAsync(match);
                _logger.LogInformation("Publishing MatchCreatedEvent for Match ID: {MatchId}", match.Id);
                await _publishEndpoint.Publish(match.AsMatchCreatedEvent());
            }

            var tournamentDto = tournament!.AsTournamentDto();
            _logger.LogInformation("Participant registered successfully for Tournament ID: {TournamentId}", request.Dto.TournamentId);
            return ApiResponse<TournamentDto>.SuccessResponse(tournamentDto, "Participant registered successfully.");
        }
    }
}
