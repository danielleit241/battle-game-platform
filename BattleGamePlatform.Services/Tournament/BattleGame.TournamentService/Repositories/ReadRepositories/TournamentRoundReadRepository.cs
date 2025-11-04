using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentRoundReadRepository : MongoRepository<TournamentRound>, ITournamentRoundReadRepository
    {
        public TournamentRoundReadRepository(IMongoDatabase database, string collectionName = "Rounds") : base(database, collectionName)
        {
        }
    }
}
