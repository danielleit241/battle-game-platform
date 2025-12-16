using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.TournamentHistoryService.Entities
{
    public class TournamentRound : IEntity
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public TournamentRoundStatus Status { get; set; }
        public Tournament Tournament { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum TournamentRoundStatus
    {
        Pending,
        Ongoing,
        Completed
    }
}
