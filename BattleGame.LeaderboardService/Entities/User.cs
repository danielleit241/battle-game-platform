using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.LeaderboardService.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
