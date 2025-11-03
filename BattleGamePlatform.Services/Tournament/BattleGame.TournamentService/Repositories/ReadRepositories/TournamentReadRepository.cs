using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities.ReadEntities;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentReadRepository : MongoRepository<TournamentRead>, ITournamentReadRepository
    {
        public TournamentReadRepository(IMongoDatabase database, string collectionName = "tournaments") : base(database, collectionName)
        {
        }
    }
}
