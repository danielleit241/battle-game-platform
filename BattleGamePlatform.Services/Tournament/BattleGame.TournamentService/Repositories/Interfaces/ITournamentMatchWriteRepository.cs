using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.Interfaces
{
    public interface ITournamentMatchWriteRepository : IBaseRepository<TournamentMatch>
    {
    }
}
