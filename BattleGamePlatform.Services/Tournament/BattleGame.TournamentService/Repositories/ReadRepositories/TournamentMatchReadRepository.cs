using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentMatchReadRepository : MongoRepository<TournamentMatch>, ITournamentMatchReadRepository
    {
        public TournamentMatchReadRepository(IMongoDatabase database, string collectionName = "Matches") : base(database, collectionName)
        {
        }
    }
}
