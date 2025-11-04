namespace BattleGame.TournamentService.IntergrationEvents
{
    public class CreatedParticipantIntergrationEvent
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string ParticipantName { get; set; } = null!;
        public bool IsEliminated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
