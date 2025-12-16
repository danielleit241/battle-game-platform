using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentHistoryService.Repositories
{
    public class TournamentMatchRepository : MongoRepository<TournamentMatch>, ITournamentMatchRepository
    {
        public TournamentMatchRepository(IMongoDatabase database, string collectionName = "Matches") : base(database, collectionName)
        {

        }
    }
}
