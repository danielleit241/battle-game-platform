using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.Interfaces
{
    public interface ITournamentMatchRepository : IBaseRepository<TournamentMatch>
    {
        Task<bool> AreAllMatchesCompletedInRound(Guid roundId);
        Task<List<TournamentMatch>> GetMatchesByRoundId(Guid roundId);
    }
}
