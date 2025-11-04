using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces
{
    public interface ITournamentRoundWriteRepository : IBaseRepository<TournamentRound>
    {
        Task CreateRoundByNumberOfParticipants(Guid tournamentId, int maxParticipants);
    }
}