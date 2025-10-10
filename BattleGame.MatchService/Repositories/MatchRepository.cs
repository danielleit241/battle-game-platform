using BattleGame.MatchService.Entities;
using BattleGame.Shared.Database.Repositories;
using MongoDB.Driver;

namespace BattleGame.MatchService.Repositories
{
    public class MatchRepository(IMongoDatabase database, string collectionName = "matches") : MongoRepository<Match>(database, collectionName), IMatchRepository
    {
    }
}
