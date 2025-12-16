using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentHistoryService.Entities;

namespace BattleGame.TournamentHistoryService.Repositories.Interfaces
{
    public interface ITournamentMatchRepository : IBaseRepository<TournamentMatch>
    {
    }
}
