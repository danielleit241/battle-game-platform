namespace BattleGame.UserService.DataAccessLayer.Infrastructure.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<OutboxEvent> OutboxEvents => Set<OutboxEvent>();
    }
}
