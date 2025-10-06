namespace BattleGame.UserService.Repositories
{
    public class RoleRepository(UserDbContext context) : PostgresRepository<Role>(context), IRoleRepository
    {
    }
}
