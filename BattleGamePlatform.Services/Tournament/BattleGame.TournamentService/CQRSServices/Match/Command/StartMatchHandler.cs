using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.Interfaces;
using MassTransit;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Match.Command
{
    public record StartMatchCommand(Guid tournamentId, Guid matchId) : IRequest<ApiResponse<MatchCompleteDto>>;
    public class StartMatchHandler : IRequestHandler<StartMatchCommand, ApiResponse<MatchCompleteDto>>
    {
        private readonly ILogger<StartMatchHandler> _logger;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentMatchRepository _tournamentMatchRepository;
        private readonly ITournamentRoundRepository _tournamentRoundRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public StartMatchHandler(
            ILogger<StartMatchHandler> logger,
            ITournamentRepository tournamentRepository,
            ITournamentMatchRepository tournamentMatchRepository,
            ITournamentRoundRepository tournamentRoundRepository,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _tournamentRepository = tournamentRepository;
            _tournamentMatchRepository = tournamentMatchRepository;
            _tournamentRoundRepository = tournamentRoundRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ApiResponse<MatchCompleteDto>> Handle(StartMatchCommand request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.GetAsync(request.tournamentId);
            if (tournament == null)
            {
                _logger.LogWarning("Tournament {TournamentId} not found", request.tournamentId);
                return ApiResponse<MatchCompleteDto>.FailureResponse("Tournament not found");
            }

            var match = await _tournamentMatchRepository.GetAsync(request.matchId);
            if (match == null)
            {
                _logger.LogWarning("Match {MatchId} not found", request.matchId);
                return ApiResponse<MatchCompleteDto>.FailureResponse("Match not found");
            }

            var randomWinnerId = new Random().Next(0, 2) == 0 ? match.Player1Id : match.Player2Id;
            match.WinnerId = randomWinnerId;
            match.UpdatedAt = DateTime.UtcNow;
            await _tournamentMatchRepository.UpdateAsync(match);
            _logger.LogInformation("Match {MatchId} completed. Winner: {WinnerId}", match.Id, randomWinnerId);

            await _publishEndpoint.Publish(match.AsMatchCompletedEvent(tournament.Id), cancellationToken);
            _logger.LogInformation("Published MatchCompletedIntergrationEvent for match {MatchId}", match.Id);

            var round = await _tournamentRoundRepository.GetAsync(r => r.Id == match.RoundId);
            if (round == null)
            {
                _logger.LogWarning("Round {RoundId} not found", match.RoundId);
                return ApiResponse<MatchCompleteDto>.FailureResponse("Round not found");
            }

            bool allMatchesCompleted = await _tournamentMatchRepository.AreAllMatchesCompletedInRound(round.Id);

            if (allMatchesCompleted)
            {
                _logger.LogInformation("All matches in round {RoundId} completed. Advancing to next round.", round.Id);

                round.Status = TournamentRoundStatus.Completed;
                round.UpdatedAt = DateTime.UtcNow;
                await _tournamentRoundRepository.UpdateAsync(round);

                await _publishEndpoint.Publish(round.AsRoundCompletedEvent(), cancellationToken);
                _logger.LogInformation("Published RoundCompletedIntergrationEvent for round {RoundId}", round.Id);

                var nextRound = await _tournamentRoundRepository.GetAsync(r =>
                    r.TournamentId == tournament.Id &&
                    r.RoundNumber == round.RoundNumber + 1);

                if (nextRound != null)
                {
                    _logger.LogInformation("Creating matches for next round {RoundNumber}", nextRound.RoundNumber);

                    var completedMatches = await _tournamentMatchRepository.GetMatchesByRoundId(round.Id);
                    var winners = completedMatches
                        .Where(m => m.WinnerId.HasValue)
                        .Select(m => m.WinnerId!.Value)
                        .ToList();

                    for (int i = 0; i < winners.Count; i += 2)
                    {
                        if (i + 1 < winners.Count)
                        {
                            var newMatch = new TournamentMatch
                            {
                                Id = Guid.NewGuid(),
                                RoundId = nextRound.Id,
                                Player1Id = winners[i],
                                Player2Id = winners[i + 1],
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _tournamentMatchRepository.AddAsync(newMatch);
                            await _publishEndpoint.Publish(newMatch.AsMatchCreatedEvent(), cancellationToken);
                            _logger.LogInformation("Created match {MatchId} for round {RoundNumber}", newMatch.Id, nextRound.RoundNumber);
                        }
                    }

                    nextRound.Status = TournamentRoundStatus.Ongoing;
                    nextRound.UpdatedAt = DateTime.UtcNow;
                    await _tournamentRoundRepository.UpdateAsync(nextRound);
                }
                else
                {
                    _logger.LogInformation("No next round found. Tournament {TournamentId} completed.", tournament.Id);
                    tournament.Status = TournamentStatus.Completed;
                    tournament.UpdatedAt = DateTime.UtcNow;
                    await _tournamentRepository.UpdateAsync(tournament);
                }
            }

            var matchCompleteDto = new MatchCompleteDto(
                match.Id,
                tournament.Id,
                randomWinnerId,
                DateTime.UtcNow
            );

            return ApiResponse<MatchCompleteDto>.SuccessResponse(matchCompleteDto, "Match completed successfully");
        }
    }
}
