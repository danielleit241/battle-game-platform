using BattleGame.TournamentService.Entities.WriteEntities;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.TournamentService.Infrastructure.Data
{
    public class TournamentWriteDbContext : DbContext
    {
        public TournamentWriteDbContext(DbContextOptions<TournamentWriteDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipant> Participants { get; set; }
        public DbSet<TournamentMatch> Matches { get; set; }
        public DbSet<TournamentRound> Rounds { get; set; }
    }
}
