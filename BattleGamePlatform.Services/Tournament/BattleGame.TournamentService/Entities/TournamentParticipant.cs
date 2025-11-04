using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.TournamentService.Entities
{
    public class TournamentParticipant : IEntity
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string ParticipantName { get; set; } = null!;
        public bool IsEliminated { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Tournament Tournament { get; set; } = null!;
    }
}
