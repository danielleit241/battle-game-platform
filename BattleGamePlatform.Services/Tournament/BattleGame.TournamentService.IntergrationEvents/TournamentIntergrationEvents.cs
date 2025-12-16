namespace BattleGame.TournamentService.IntergrationEvents
{
    public class CreatedTournamentIntergrationEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MaxParticipants { get; set; }
        public Guid GameId { get; set; }
        public TournamentStatus Status { get; set; }
        public TournamentFormat Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
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
