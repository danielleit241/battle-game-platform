using BattleGame.LeaderboardService.Dtos;
using BattleGame.MessageBus.Events;
using BattleGame.Shared.Common;

namespace BattleGame.LeaderboardService.Services
{
    public interface ILeaderboardServices
    {
        Task UpSertLeaderboard(MatchCompletedEvent @event);
        Task<ApiResponse<IReadOnlyCollection<LeaderboardWithGameDto>>> GetAllLeaderboard();
        Task<ApiResponse<LeaderboardWithGameDto>> GetTopXLeaderboardByGameId(Guid gameId, int top);
    }
}
