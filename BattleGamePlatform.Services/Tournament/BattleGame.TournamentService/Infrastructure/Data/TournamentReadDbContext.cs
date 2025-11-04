using BattleGame.TournamentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.TournamentService.Infrastructure.Data
{
    public class TournamentReadDbContext : DbContext
    {
        public TournamentReadDbContext(DbContextOptions<TournamentReadDbContext> options) : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipant> Participants { get; set; }
        public DbSet<TournamentMatch> Matches { get; set; }
        public DbSet<TournamentRound> Rounds { get; set; }
    }
}
