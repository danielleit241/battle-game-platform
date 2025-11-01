namespace BattleGame.MatchService.Services
{
    public interface IMatchLogService
    {
        Task<ApiResponse<IReadOnlyCollection<MatchDto>>> GetMatchesByUserIdAsync(Guid userId);
        Task CreateMatch(GameCompletedEvent @event);
    }
}
