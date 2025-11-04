using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces
{
    public interface ITournamentMatchWriteRepository : IBaseRepository<TournamentMatch>
    {
        Task AddMatchesByRoundId(Guid roundId, Guid player1, Guid player2);
    }
}
