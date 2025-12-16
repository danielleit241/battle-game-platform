using BattleGame.TournamentHistoryService.Entities;

namespace BattleGame.TournamentHistoryService.Dtos
{
    public class TournamentSnapshotDto
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
        public List<ParticipantDto> Participants { get; set; } = new();
        public List<RoundDto> Rounds { get; set; } = new();
    }

    public class ParticipantDto
    {
        public string ParticipantName { get; set; } = null!;
        public bool IsEliminated { get; set; }
    }

    public class RoundDto
    {
        public int RoundNumber { get; set; }
        public TournamentRoundStatus Status { get; set; }
        public List<MatchDto> Matches { get; set; } = new();
    }

    public class MatchDto
    {
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public Guid? WinnerId { get; set; }
    }
}

