namespace BattleGame.TournamentService.IntergrationEvents
{
    public class CreatedTournamentIntergrationEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MaxParticipants { get; set; }
        public Guid GameId { get; set; }
        public int Status { get; set; }
        public int Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
