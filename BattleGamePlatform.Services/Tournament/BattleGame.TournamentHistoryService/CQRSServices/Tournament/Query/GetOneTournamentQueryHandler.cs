using BattleGame.Shared.Common;
using BattleGame.TournamentHistoryService.Dtos;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MediatR;

namespace BattleGame.TournamentHistoryService.CQRSServices.Tournament.Query
{
    public record GetOneTournamentQuery(Guid TournamentId) : IRequest<ApiResponse<TournamentSnapshotDto>>;
    public class GetOneTournamentQueryHandler : IRequestHandler<GetOneTournamentQuery, ApiResponse<TournamentSnapshotDto>>
    {
        private readonly ILogger<GetOneTournamentQueryHandler> _logger;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentParticipantRepository _tournamentParticipantRepository;
        private readonly ITournamentRoundRepository _tournamentRoundRepository;
        private readonly ITournamentMatchRepository _tournamentMatchRepository;

        public GetOneTournamentQueryHandler(
            ILogger<GetOneTournamentQueryHandler> logger,
            ITournamentRepository tournamentRepository,
            ITournamentParticipantRepository tournamentParticipantRepository,
            ITournamentRoundRepository tournamentRoundRepository,
            ITournamentMatchRepository tournamentMatchRepository)
        {
            _logger = logger;
            _tournamentRepository = tournamentRepository;
            _tournamentParticipantRepository = tournamentParticipantRepository;
            _tournamentRoundRepository = tournamentRoundRepository;
            _tournamentMatchRepository = tournamentMatchRepository;
        }

        public async Task<ApiResponse<TournamentSnapshotDto>> Handle(GetOneTournamentQuery request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.GetAsync(request.TournamentId);
            if (tournament == null)
            {
                _logger.LogWarning("Tournament with ID {TournamentId} not found.", request.TournamentId);
                return ApiResponse<TournamentSnapshotDto>.FailureResponse("Tournament not found.");
            }

            var participantsTask = _tournamentParticipantRepository.GetAllAsync(p => p.TournamentId == request.TournamentId);
            var roundsTask = _tournamentRoundRepository.GetAllAsync(r => r.TournamentId == request.TournamentId);

            await Task.WhenAll(participantsTask, roundsTask);

            var participants = await participantsTask;
            var rounds = await roundsTask;

            IEnumerable<TournamentMatch> matches = [];

            if (rounds.Any())
            {
                var roundIds = rounds.Select(r => r.Id).ToList();
                matches = await _tournamentMatchRepository.GetAllAsync(m => roundIds.Contains(m.RoundId));
            }

            var dto = tournament.ToDto(participants, rounds, matches);
            return ApiResponse<TournamentSnapshotDto>.SuccessResponse(dto);
        }
    }
}
