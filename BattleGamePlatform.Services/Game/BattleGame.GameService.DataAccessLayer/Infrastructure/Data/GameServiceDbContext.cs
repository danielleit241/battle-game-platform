namespace BattleGame.GameService.DataAccessLayer.Infrastructure.Data
{
    public class GameServiceDbContext : DbContext
    {
        public GameServiceDbContext(DbContextOptions<GameServiceDbContext> options) : base(options)
        {
        }
        public DbSet<Common.Entities.Game> Games { get; set; }
    }
}
