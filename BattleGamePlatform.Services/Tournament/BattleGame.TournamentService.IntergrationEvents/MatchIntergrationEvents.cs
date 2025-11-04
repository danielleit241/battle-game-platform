namespace BattleGame.TournamentService.IntergrationEvents
{
    public class CreatedMatchIntergrationEvent
    {
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public Guid? WinnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
