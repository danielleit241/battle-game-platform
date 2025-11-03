namespace BattleGame.TournamentService.Entities.ReadEntities
{
    public class MatchRead
    {
        public Guid Id { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public Guid? WinnerId { get; set; }
    }
}
