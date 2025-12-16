using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.TournamentService.Repositories
{
    public class TournamentParticipantWriteRepository : PostgresRepository<TournamentParticipant>, ITournamentParticipantWriteRepository
    {
        public TournamentParticipantWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }

        public async Task<bool> IsEnoughParticipantInTournament(Guid tournamentId, int maxParticipants)
        {
            var participantCount = await _dbSet.CountAsync(tp => tp.TournamentId == tournamentId && !tp.IsEliminated);
            if (participantCount <= maxParticipants)
                return false;
            return true;
        }
    }
}