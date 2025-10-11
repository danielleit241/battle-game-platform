using BattleGame.LeaderboardService.Clients;
using BattleGame.LeaderboardService.Dtos;
using BattleGame.LeaderboardService.Repositories;
using BattleGame.MessageBus.Events;
using BattleGame.Shared.Common;

namespace BattleGame.LeaderboardService.Services
{
    public class LeaderboardServices : ILeaderboardServices
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IGameRepository _gameRepository;
        private readonly UserClient _userClient;
        public LeaderboardServices(ILeaderboardRepository leaderboardRepository, IGameRepository gameRepository, UserClient userClient)
        {
            _leaderboardRepository = leaderboardRepository;
            _gameRepository = gameRepository;
            _userClient = userClient;
        }
        public async Task<ApiResponse<IReadOnlyCollection<LeaderboardWithGameDto>>> GetAllLeaderboard()
        {
            var games = await _gameRepository.GetAllAsync();
            if (games is null || !games.Any())
            {
                return ApiResponse<IReadOnlyCollection<LeaderboardWithGameDto>>.FailureResponse("No games found", 404);
            }
            var leaderboards = await _leaderboardRepository.GetAllAsync();
            if (leaderboards is null || !leaderboards.Any())
            {
                return ApiResponse<IReadOnlyCollection<LeaderboardWithGameDto>>.FailureResponse("No leaderboards found", 404);
            }
            var result = games.Select(game => new LeaderboardWithGameDto(
                new GameDto(game.Id, game.GameName),
                leaderboards
                    .Where(leaderboard => leaderboard.GameId == game.Id)
                    .OrderByDescending(leaderboard => leaderboard.TotalScore)
                    .Take(10)
                    .Select(leaderboard => new LeaderboardDto(
                        leaderboard.Id,
                        leaderboard.UserId,
                        leaderboard.Username,
                        leaderboard.TotalScore,
                        leaderboard.CreatedAt,
                        leaderboard.UpdatedAt
                    )).ToList()
            )).ToList();

            return ApiResponse<IReadOnlyCollection<LeaderboardWithGameDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<LeaderboardWithGameDto>> GetTopXLeaderboardByGameId(Guid gameId, int top)
        {
            var game = await _gameRepository.GetAsync(game => game.Id == gameId);
            if (game is null)
            {
                return ApiResponse<LeaderboardWithGameDto>.FailureResponse("Game not found", 404);
            }
            var leaderboards = await _leaderboardRepository.GetAllAsync(leaderboard => leaderboard.GameId == gameId);

            if (leaderboards is null)
            {
                return ApiResponse<LeaderboardWithGameDto>.FailureResponse("No leaderboards found for this game", 404);
            }
            var result = new LeaderboardWithGameDto(
                new GameDto(game.Id, game.GameName),
                leaderboards
                    .Where(leaderboard => leaderboard.GameId == game.Id)
                    .OrderByDescending(leaderboard => leaderboard.TotalScore)
                    .Take(top)
                    .Select(leaderboard => new LeaderboardDto(
                        leaderboard.Id,
                        leaderboard.UserId,
                        leaderboard.Username,
                        leaderboard.TotalScore,
                        leaderboard.CreatedAt,
                        leaderboard.UpdatedAt
                    )).ToList()
            );

            return ApiResponse<LeaderboardWithGameDto>.SuccessResponse(result);
        }

        public async Task UpSertLeaderboard(MatchCompletedEvent @event)
        {
            var leaderboard = await _leaderboardRepository.GetAsync(leaderboard => leaderboard.GameId == @event.GameId && leaderboard.UserId == @event.UserId);
            if (leaderboard is not null)
            {
                leaderboard.TotalScore += @event.Score;
                leaderboard.UpdatedAt = DateTime.UtcNow;
                var user = await _userClient.GetUserByIdAsync(@event.UserId);
                if (user.Username != leaderboard.Username)
                {
                    leaderboard.Username = user.Username;
                }
                await _leaderboardRepository.UpdateAsync(leaderboard);
            }
            else
            {
                var newLeaderboard = @event.AsEntity();
                var user = await _userClient.GetUserByIdAsync(@event.UserId);
                if (user is not null)
                {
                    newLeaderboard.Username = user.Username;
                }
                else
                {
                    newLeaderboard.Username = "Unknown";
                }
                await _leaderboardRepository.AddAsync(newLeaderboard);
            }
        }
    }
}
