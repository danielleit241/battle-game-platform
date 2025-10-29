using BattleGame.Shared.Database.Repositories;
using BattleGame.UserService.DataAccessLayer.Infrastructure.Data;
using BattleGame.UserService.DataAccessLayer.Repositories.Abstractions;

namespace BattleGame.UserService.DataAccessLayer.Repositories.Implementations
{
    public class UserRepository : PostgresRepository<User>, IUserRepository
    {
        public UserRepository(UserDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyCollection<User>> GetAllUserIncludeRoleAsync()
            => await _dbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .ToListAsync();

        public Task<User?> GetUserIncludeRoleAsync(Guid id)
            => _dbSet.AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

    }
}
