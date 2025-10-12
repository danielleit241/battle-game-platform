using BattleGame.MatchService.Clients;
using BattleGame.MatchService.Dtos;
using BattleGame.MatchService.Entities;
using BattleGame.MatchService.Repositories;
using BattleGame.MessageBus.Events;
using BattleGame.Shared.Common;
using MassTransit;
using MassTransit.Testing;

namespace BattleGame.MatchService.Services
{
    public class MatchLogService(
        IMatchRepository repository,
        UserClient userClient,
        GameClient gameClient,
        IPublishEndpoint publishEndpoint) : IMatchLogService
    {
        public async Task CreateMatch(GameCompletedEvent @event)
        {
            var match = new Match
            {
                Id = Guid.NewGuid(),
                GameId = @event.GameId,
                UserId = @event.UserId,
                Timestamp = @event.CompletedAt,
                Score = (int)Math.Abs(new Random().NextInt64(0, 100))
            };

            await repository.AddAsync(match);

            var matchCompletedEvent = new MatchCompletedEvent(
                match.Id,
                match.UserId,
                match.GameId,
                match.Score,
                match.CreatedAt);

            await publishEndpoint.Publish(matchCompletedEvent);
        }

        public async Task<ApiResponse<IReadOnlyCollection<MatchDto>>> GetMatchesByUserIdAsync(Guid userId)
        {
            var user = await userClient.GetUserByIdAsync(userId);
            if (user is null)
            {
                return ApiResponse<IReadOnlyCollection<MatchDto>>.FailureResponse("User not found");
            }

            var matches = await repository.GetAllAsync(u => u.UserId == userId);
            if (matches is null || !matches.Any())
            {
                return ApiResponse<IReadOnlyCollection<MatchDto>>.FailureResponse("No matches found for the user");
            }

            var games = await gameClient.GetAllGamesAsync();

            var matchWithGameDtos = matches.Select(m =>
            {
                var game = games.Where(g => g.Id == m.GameId).FirstOrDefault();
                return new MatchDto(
                    m.Id,
                    m.UserId,
                    user.Username,
                    new List<MatchGameDto>
                    {
                        new
                        (
                            game!.Id,
                            game.Name,
                            m.Score,
                            m.Timestamp
                        )
                    }.AsReadOnly());
            }).ToList().AsReadOnly();

            return ApiResponse<IReadOnlyCollection<MatchDto>>.SuccessResponse(matchWithGameDtos);
        }
    }
}
