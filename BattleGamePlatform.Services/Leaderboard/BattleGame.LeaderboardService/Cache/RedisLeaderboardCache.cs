using StackExchange.Redis;

namespace BattleGame.LeaderboardService.Cache
{
    public class RedisLeaderboardCache
    {
        private readonly StackExchange.Redis.IDatabase _database;
        private const string LeaderboardKey = "leaderboard";

        public RedisLeaderboardCache(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<List<(Guid UserId, double Score)>> GetTopAsync(Guid gameId, int top = 10)
        {
            var key = LeaderboardKey + gameId;
            var results = await _database.SortedSetRangeByRankWithScoresAsync(key, 0, top - 1, Order.Descending);
            return results.Select(r => (Guid.Parse(r.Element!), r.Score)).ToList();
        }

        public async Task UpdateScoreAsync(Guid gameId, Guid userId, int score)
        {
            var key = LeaderboardKey + gameId;
            await _database.SortedSetAddAsync(key, userId.ToString(), score);
            await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(30));
        }

    }
}
