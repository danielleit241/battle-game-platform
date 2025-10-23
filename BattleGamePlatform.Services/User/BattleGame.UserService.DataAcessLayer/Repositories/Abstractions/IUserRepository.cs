using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.UserService.DataAcessLayer.Repositories.Abstractions
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IReadOnlyCollection<User>> GetAllUserIncludeRoleAsync();
        Task<User?> GetUserIncludeRoleAsync(Guid id);
    }
}
