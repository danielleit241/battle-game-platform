using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BattleGame.TournamentHistoryService.Repositories
{
    public class TournamentParticipantRepository : MongoRepository<TournamentParticipant>, ITournamentParticipantRepository
    {
        public TournamentParticipantRepository(IMongoDatabase database, string collectionName = "Participants") : base(database, collectionName)
        {
        }
    }
}