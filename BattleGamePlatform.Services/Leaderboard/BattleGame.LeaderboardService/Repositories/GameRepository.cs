namespace BattleGame.LeaderboardService.Repositories
{
    public class GameRepository : MongoRepository<Game>, IGameRepository
    {
        public GameRepository(IMongoDatabase database, string collectionName = "games") : base(database, collectionName)
        {
        }
    }
}
