namespace BattleGame.GameService.DataAccessLayer.Repositories.Implementations
{
    public class GameRepository : PostgresRepository<Game>, IGameRepository
    {
        public GameRepository(GameServiceDbContext context) : base(context)
        {
        }

        public async Task<Game?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(g => g.Name == name);
        }
    }
}
