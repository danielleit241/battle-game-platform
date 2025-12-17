using BattleGame.Shared.Common;
using BattleGame.TournamentHistoryService.Dtos;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MediatR;

namespace BattleGame.TournamentHistoryService.CQRSServices.Tournament.Query
{
    public record GetAllTournamentQuery : IRequest<ApiResponse<IEnumerable<TournamentSnapshotDto>>>;
    public class GetAllTournamentQueryHandler : IRequestHandler<GetAllTournamentQuery, ApiResponse<IEnumerable<TournamentSnapshotDto>>>
    {
        private readonly ILogger<GetAllTournamentQueryHandler> _logger;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentParticipantRepository _tournamentParticipantRepository;
        private readonly ITournamentRoundRepository _tournamentRoundRepository;
        private readonly ITournamentMatchRepository _tournamentMatchRepository;

        public GetAllTournamentQueryHandler(
            ILogger<GetAllTournamentQueryHandler> logger,
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

        public async Task<ApiResponse<IEnumerable<TournamentSnapshotDto>>> Handle(GetAllTournamentQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository.GetAllAsync();
            if (tournaments == null || !tournaments.Any())
            {
                _logger.LogWarning("No tournaments found.");
                return ApiResponse<IEnumerable<TournamentSnapshotDto>>.FailureResponse("No tournaments found.");
            }

            var tournamentIds = tournaments.Select(t => t.Id).ToList();
            var participantsTask = _tournamentParticipantRepository.GetAllAsync(p => tournamentIds.Contains(p.TournamentId));
            var roundsTask = _tournamentRoundRepository.GetAllAsync(r => tournamentIds.Contains(r.TournamentId));

            await Task.WhenAll(participantsTask, roundsTask);

            var allParticipants = await participantsTask;
            var allRounds = await roundsTask;

            var roundIds = allRounds.Select(r => r.Id).ToList();
            var allMatches = await _tournamentMatchRepository.GetAllAsync(m => roundIds.Contains(m.RoundId));

            var response = new List<TournamentSnapshotDto>();

            foreach (var tournament in tournaments)
            {
                var tParticipants = allParticipants.Where(p => p.TournamentId == tournament.Id);
                var tRounds = allRounds.Where(r => r.TournamentId == tournament.Id);
                var tRoundIds = tRounds.Select(r => r.Id).ToHashSet();
                var tMatches = allMatches.Where(m => tRoundIds.Contains(m.RoundId));

                response.Add(tournament.ToDto(tParticipants, tRounds, tMatches));
            }

            _logger.LogInformation("Retrieved {Count} tournaments.", response.Count);
            return ApiResponse<IEnumerable<TournamentSnapshotDto>>.SuccessResponse(response);
        }
    }
}