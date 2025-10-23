using BattleGame.MatchService.Dtos;
using BattleGame.MessageBus.Events;
using BattleGame.Shared.Common;

namespace BattleGame.MatchService.Services
{
    public interface IMatchLogService
    {
        Task<ApiResponse<IReadOnlyCollection<MatchDto>>> GetMatchesByUserIdAsync(Guid userId);
        Task CreateMatch(GameCompletedEvent @event);
    }
}
