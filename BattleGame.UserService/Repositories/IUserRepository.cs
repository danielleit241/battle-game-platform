namespace BattleGame.UserService.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IReadOnlyCollection<User>> GetAllUserIncludeRoleAsync();
        Task<User?> GetUserIncludeRoleAsync(Guid id);
    }
}
