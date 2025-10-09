using BattleGame.MatchService.Entities;
using BattleGame.Shared.Database.Repositories;
using MongoDB.Driver;

namespace BattleGame.MatchService.Repositories
{
    public class MatchRepository : MongoRepository<Match>, IMatchRepository
    {
        public MatchRepository(IMongoDatabase database, string collectionName = "matches") : base(database, collectionName)
        {
        }
    }
}
