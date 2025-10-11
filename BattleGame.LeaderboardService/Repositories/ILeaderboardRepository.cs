using BattleGame.LeaderboardService.Entities;
using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.LeaderboardService.Repositories
{
    public interface ILeaderboardRepository : IBaseRepository<Leaderboard>
    {
    }
}
