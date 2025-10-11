using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.LeaderboardService.Entities
{
    public class Leaderboard : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public Guid GameId { get; set; }
        public int TotalScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
