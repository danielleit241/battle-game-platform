using BattleGame.Shared.Database.Repositories;
using BattleGame.UserService.DataAccessLayer.Infrastructure.Data;
using BattleGame.UserService.DataAccessLayer.Repositories.Abstractions;

namespace BattleGame.UserService.DataAccessLayer.Repositories.Implementations
{
    public class RoleRepository(UserDbContext context) : PostgresRepository<Role>(context), IRoleRepository
    {
    }
}
