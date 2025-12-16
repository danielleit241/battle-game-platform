using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentHistoryService.Repositories
{
    public class TournamentRoundRepository : MongoRepository<TournamentRound>, ITournamentRoundRepository
    {
        public TournamentRoundRepository(IMongoDatabase database, string collectionName = "Rounds") : base(database, collectionName)
        {
        }
    }
}
