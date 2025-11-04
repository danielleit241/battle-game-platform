using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces
{
    public interface ITournamentRoundWriteRepository : IBaseRepository<TournamentRound>
    {
        Task<IEnumerable<TournamentRound>> GenerateRoundsForTournamentAsync(Guid tournamentId, int maxParticipants);
        Task<TournamentRound> GetRoundIsNotCompletedMatchesByTournamentId(Guid tournamentId);
    }
}