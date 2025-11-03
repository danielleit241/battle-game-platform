using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.TournamentService.Entities.WriteEntities
{
    public class Tournament : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MaxParticipants { get; set; }
        public string GameId { get; set; } = null!;
        public TournamentStatus Status { get; set; }
        public TournamentFormat Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<TournamentParticipant> Participants { get; set; } = [];
        public ICollection<TournamentRound>? Rounds { get; set; }
    }

    public enum TournamentStatus
    {
        Upcoming,
        Ongoing,
        Completed,
        Cancelled
    }

    public enum TournamentFormat
    {
        SingleElimination,
        DoubleElimination
    }
}
