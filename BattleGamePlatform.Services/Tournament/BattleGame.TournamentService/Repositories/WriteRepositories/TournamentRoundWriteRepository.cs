using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentRoundWriteRepository : PostgresRepository<TournamentRound>, ITournamentRoundWriteRepository
    {
        public TournamentRoundWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TournamentRound>> GenerateRoundsForTournamentAsync(Guid tournamentId, int maxParticipants)
        {
            var rounds = new List<TournamentRound>();
            int totalRounds = (int)Math.Log2(maxParticipants);
            for (int i = 1; i <= totalRounds; i++)
            {
                var round = new TournamentRound
                {
                    Id = Guid.NewGuid(),
                    TournamentId = tournamentId,
                    RoundNumber = i,
                    CreatedAt = DateTime.UtcNow
                };
            }
            await _dbSet.AddRangeAsync(rounds);
            return rounds;
        }

        public async Task<TournamentRound> GetRoundIsNotCompletedMatchesByTournamentId(Guid tournamentId)
        {
            var round = await _dbSet.Where(r => r.TournamentId == tournamentId && r.Status != TournamentRoundStatus.Completed)
                             .OrderBy(r => r.RoundNumber)
                             .FirstAsync();
            return round;
        }

    }
}
