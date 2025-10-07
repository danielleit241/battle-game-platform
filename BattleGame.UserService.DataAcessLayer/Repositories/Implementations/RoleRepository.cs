using BattleGame.Shared.Database.Repositories;

namespace BattleGame.UserService.DataAcessLayer.Repositories.Implementations
{
    public class RoleRepository(UserDbContext context) : PostgresRepository<Role>(context), IRoleRepository
    {
    }
}
