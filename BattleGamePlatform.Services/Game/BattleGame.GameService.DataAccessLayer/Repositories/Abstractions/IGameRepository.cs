using BattleGame.GameService.Common.Entities;
using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.GameService.DataAccessLayer.Repositories.Abstractions
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game?> GetByNameAsync(string name);
    }
}
