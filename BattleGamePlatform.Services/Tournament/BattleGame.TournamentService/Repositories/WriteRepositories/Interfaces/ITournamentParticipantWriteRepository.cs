using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces
{
    public interface ITournamentParticipantWriteRepository : IBaseRepository<TournamentParticipant>
    {
        Task<bool> IsEnoughParticipantInTournament(Guid tournamentId, int maxParticipants);
    }
}
