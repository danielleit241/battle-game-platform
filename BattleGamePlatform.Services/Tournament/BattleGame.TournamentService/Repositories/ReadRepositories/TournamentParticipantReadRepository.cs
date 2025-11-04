using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentParticipantReadRepository : MongoRepository<TournamentParticipant>, ITournamentParticipantReadRepository
    {
        public TournamentParticipantReadRepository(IMongoDatabase database, string collectionName = "Participants") : base(database, collectionName)
        {
        }
    }
}
