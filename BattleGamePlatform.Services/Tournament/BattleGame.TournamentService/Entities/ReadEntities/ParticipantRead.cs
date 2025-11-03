namespace BattleGame.TournamentService.Entities.ReadEntities
{
    public class ParticipantRead
    {
        public Guid Id { get; set; }
        public string ParticipantName { get; set; } = null!;
        public bool IsEliminated { get; set; } = false;
    }
}
