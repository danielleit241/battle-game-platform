namespace BattleGame.LeaderboardService.Services
{
    public class LeaderboardServices : ILeaderboardServices
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly RedisLeaderboardCache _cache;
        public LeaderboardServices(ILeaderboardRepository leaderboardRepository, IGameRepository gameRepository, IUserRepository userRepository, RedisLeaderboardCache cache)
        {
            _leaderboardRepository = leaderboardRepository;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _cache = cache;
        }
        public async Task<ApiResponse<IReadOnlyCollection<LeaderboardResponseDto>>> GetAllLeaderboard()
        {
            var games = await _gameRepository.GetAllAsync();
            if (games is null || !games.Any())
            {
                return ApiResponse<IReadOnlyCollection<LeaderboardResponseDto>>.FailureResponse("No games found", 404);
            }
            var leaderboards = await _leaderboardRepository.GetAllAsync();
            if (leaderboards is null || !leaderboards.Any())
            {
                return ApiResponse<IReadOnlyCollection<LeaderboardResponseDto>>.FailureResponse("No leaderboards found", 404);
            }
            var result = games.Select(game => new LeaderboardResponseDto(
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

            return ApiResponse<IReadOnlyCollection<LeaderboardResponseDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<LeaderboardResponseDto>> GetTopXLeaderboardByGameId(Guid gameId, int top)
        {
            var game = await _gameRepository.GetAsync(game => game.Id == gameId);
            if (game is null)
            {
                return ApiResponse<LeaderboardResponseDto>.FailureResponse("Game not found", 404);
            }

            var cachedLeaderboards = await _cache.GetTopAsync(gameId, top);
            if (cachedLeaderboards is not null && cachedLeaderboards.Count != 0)
            {
                var topPlayers = (await Task.WhenAll(
                    cachedLeaderboards.Select(async x =>
                    {
                        var user = await _userRepository.GetAsync(u => u.Id == x.UserId);
                        return new LeaderboardDto(
                            Guid.Empty,
                            x.UserId,
                            user?.Username ?? "Unknown",
                            (int)x.Score,
                            DateTime.MinValue,
                            DateTime.MinValue
                        );
                    })
                )).ToList();

                return ApiResponse<LeaderboardResponseDto>.SuccessResponse(
                    new LeaderboardResponseDto(new GameDto(game.Id, game.GameName), topPlayers)
                );
            }

            var leaderboards = await _leaderboardRepository.GetAllAsync(leaderboard => leaderboard.GameId == gameId);
            if (leaderboards is null)
            {
                return ApiResponse<LeaderboardResponseDto>.FailureResponse("No leaderboards found for this game", 404);
            }
            var result = new LeaderboardResponseDto(
                new GameDto(game.Id, game.GameName),
                [.. leaderboards
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
                    ))]
            );

            return ApiResponse<LeaderboardResponseDto>.SuccessResponse(result);
        }

        public async Task UpSertLeaderboard(MatchCompletedEvent @event)
        {
            var user = await _userRepository.GetAsync(user => user.Id == @event.UserId);
            if (user is null)
            {
                return;
            }

            var leaderboard = await _leaderboardRepository.GetAsync(leaderboard => leaderboard.GameId == @event.GameId && leaderboard.UserId == @event.UserId);

            if (leaderboard is not null)
            {
                leaderboard.TotalScore += @event.Score;
                leaderboard.UpdatedAt = DateTime.UtcNow;
                leaderboard.Username = user.Username ?? "Unknown";
                await _leaderboardRepository.UpdateAsync(leaderboard);
            }
            else
            {
                var newLeaderboard = @event.AsEntity();
                newLeaderboard.Username = user.Username ?? "Unknown";
                await _leaderboardRepository.AddAsync(newLeaderboard);
            }

            if (leaderboard is not null)
                await _cache.UpdateScoreAsync(@event.GameId, @event.UserId, leaderboard.TotalScore);
        }
    }
}
