using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.TournamentService.Entities.ReadEntities
{
    public class TournamentRead : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxParticipants { get; set; }
        public Guid GameId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public List<ParticipantRead> Participants { get; set; } = new();
        public List<RoundRead> Rounds { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
