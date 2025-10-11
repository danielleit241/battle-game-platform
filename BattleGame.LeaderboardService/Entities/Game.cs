using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.LeaderboardService.Entities
{
    public class Game : IEntity
    {
        public Guid Id { get; set; }
        public string GameName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
