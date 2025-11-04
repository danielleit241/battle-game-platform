using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentParticipantReadRepository : MongoRepository<TournamentParticipant>, ITournamentParticipantWriteRepository
    {
        public TournamentParticipantReadRepository(IMongoDatabase database, string collectionName = "Participants") : base(database, collectionName)
        {
        }
    }
}
