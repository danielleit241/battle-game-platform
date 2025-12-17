using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.TournamentService.Repositories
{
    public class TournamentMatchRepository : PostgresRepository<TournamentMatch>, ITournamentMatchRepository
    {
        public TournamentMatchRepository(TournamentWriteDbContext context) : base(context)
        {
        }

        public async Task<bool> AreAllMatchesCompletedInRound(Guid roundId)
        {
            return await _dbSet
                .Where(m => m.RoundId == roundId)
                .AllAsync(m => m.WinnerId != null);
        }

        public async Task<List<TournamentMatch>> GetMatchesByRoundId(Guid roundId)
        {
            return await _dbSet
                .Where(m => m.RoundId == roundId)
                .ToListAsync();
        }
    }
}
