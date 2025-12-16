using BattleGame.Shared.Database.Abstractions;
using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Repositories.Interfaces
{
    public interface ITournamentParticipantRepository : IBaseRepository<TournamentParticipant>
    {
        Task<bool> IsEnoughParticipantInTournament(Guid tournamentId, int maxParticipants);
    }
}
