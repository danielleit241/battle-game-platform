namespace BattleGame.TournamentService.IntergrationEvents
{
    public class CreatedRoundIntergrationEvent
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public TournamentRoundStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RoundCompletedIntergrationEvent
    {
        public Guid RoundId { get; set; }
        public Guid TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public enum TournamentRoundStatus
    {
        Pending,
        Ongoing,
        Completed
    }
}
