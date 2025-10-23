using BattleGame.LeaderboardService.Entities;
using BattleGame.Shared.Database.Repositories;
using MongoDB.Driver;

namespace BattleGame.LeaderboardService.Repositories
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase database, string collectionName = "users") : base(database, collectionName)
        {
        }
    }
}
