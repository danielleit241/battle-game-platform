using BattleGame.LeaderboardService.Entities;
using BattleGame.Shared.Database.Repositories;
using MongoDB.Driver;

namespace BattleGame.LeaderboardService.Repositories
{
    public class LeaderboardRepository : MongoRepository<Leaderboard>, ILeaderboardRepository
    {
        public LeaderboardRepository(IMongoDatabase database, string collectionName = "leaderboards") : base(database, collectionName)
        {
        }
    }
}
