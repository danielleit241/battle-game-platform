namespace BattleGame.LeaderboardService.Services
{
    public interface ILeaderboardServices
    {
        Task UpSertLeaderboard(MatchCompletedEvent @event);
        Task<ApiResponse<IReadOnlyCollection<LeaderboardResponseDto>>> GetAllLeaderboard();
        Task<ApiResponse<LeaderboardResponseDto>> GetTopXLeaderboardByGameId(Guid gameId, int top);
    }
}
