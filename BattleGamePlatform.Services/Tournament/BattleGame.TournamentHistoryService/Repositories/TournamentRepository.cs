using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentHistoryService.Repositories
{
    public class TournamentRepository : MongoRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(IMongoDatabase database, string collectionName = "Tournaments") : base(database, collectionName)
        {
        }
    }
}
