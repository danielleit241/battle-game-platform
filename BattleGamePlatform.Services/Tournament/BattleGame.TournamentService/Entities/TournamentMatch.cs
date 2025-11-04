using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.TournamentService.Entities
{
    public class TournamentMatch : IEntity
    {
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public Guid? WinnerId { get; set; }
        public TournamentRound? Round { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
