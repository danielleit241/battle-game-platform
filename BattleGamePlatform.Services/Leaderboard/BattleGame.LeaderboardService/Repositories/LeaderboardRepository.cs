namespace BattleGame.LeaderboardService.Repositories
{
    public class LeaderboardRepository : MongoRepository<Leaderboard>, ILeaderboardRepository
    {
        public LeaderboardRepository(IMongoDatabase database, string collectionName = "leaderboards") : base(database, collectionName)
        {
        }
    }
}
