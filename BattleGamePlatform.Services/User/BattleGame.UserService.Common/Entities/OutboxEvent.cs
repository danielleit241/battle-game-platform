using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.UserService.Common.Entities
{
    public class OutboxEvent : IEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ProcessedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
