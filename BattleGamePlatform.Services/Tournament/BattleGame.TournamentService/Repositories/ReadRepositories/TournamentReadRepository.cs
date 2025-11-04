using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentReadRepository : MongoRepository<Tournament>, ITournamentReadRepository
    {
        public TournamentReadRepository(IMongoDatabase database, string collectionName = "Tournaments") : base(database, collectionName)
        {
        }
    }
}
