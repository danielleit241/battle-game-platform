namespace BattleGame.TournamentService.Entities.ReadEntities
{
    public class RoundRead
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public List<MatchRead> Matches { get; set; } = new();
    }
}
