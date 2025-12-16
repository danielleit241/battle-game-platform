using BattleGame.TournamentHistoryService.Dtos;
using BattleGame.TournamentHistoryService.Entities;
using BattleGame.TournamentService.IntergrationEvents;

namespace BattleGame.TournamentHistoryService
{
    public static class MappingExtensions
    {
        public static Tournament ToEntity(this CreatedTournamentIntergrationEvent @event)
        {
            var tournament = new Tournament
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                MaxParticipants = @event.MaxParticipants,
                GameId = @event.GameId,
                Status = (Entities.TournamentStatus)@event.Status,
                Format = (Entities.TournamentFormat)@event.Format,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                CreatedAt = @event.CreatedAt,
                UpdatedAt = @event.UpdatedAt,
            };
            return tournament;
        }

        public static TournamentParticipant ToEntity(this CreatedParticipantIntergrationEvent @event)
        {
            var participant = new TournamentParticipant
            {
                Id = @event.Id,
                TournamentId = @event.TournamentId,
                ParticipantName = @event.ParticipantName,
                IsEliminated = false,
                CreatedAt = @event.CreatedAt,
                UpdatedAt = @event.UpdatedAt
            };
            return participant;
        }

        public static TournamentRound ToEntity(this CreatedRoundIntergrationEvent @event)
        {
            var round = new TournamentRound
            {
                Id = @event.Id,
                TournamentId = @event.TournamentId,
                RoundNumber = @event.RoundNumber,
                Status = (Entities.TournamentRoundStatus)@event.Status,
                CreatedAt = @event.CreatedAt,
                UpdatedAt = @event.UpdatedAt
            };
            return round;
        }

        public static TournamentMatch ToEntity(this CreatedMatchIntergrationEvent @event)
        {
            var match = new TournamentMatch
            {
                Id = @event.Id,
                RoundId = @event.RoundId,
                Player1Id = @event.Player1Id,
                Player2Id = @event.Player2Id,
                WinnerId = @event.WinnerId,
                CreatedAt = @event.CreatedAt,
                UpdatedAt = @event.UpdatedAt
            };
            return match;
        }

        public static TournamentSnapshotDto ToDto(this Tournament snapshot, IEnumerable<TournamentParticipant> participants, IEnumerable<TournamentRound> rounds, IEnumerable<TournamentMatch> matchs)
        {
            return new TournamentSnapshotDto
            {
                Id = snapshot.Id,
                Name = snapshot.Name,
                Description = snapshot.Description,
                MaxParticipants = snapshot.MaxParticipants,
                GameId = snapshot.GameId,
                Status = snapshot.Status,
                Format = snapshot.Format,
                StartDate = snapshot.StartDate,
                EndDate = snapshot.EndDate,
                Participants = [.. participants.Select(p => new ParticipantDto
                {
                    ParticipantName = p.ParticipantName,
                    IsEliminated = p.IsEliminated
                })],
                Rounds = [.. rounds.Select(r => new RoundDto
                {
                    RoundNumber = r.RoundNumber,
                    Status = r.Status,
                    Matches = [.. matchs.Select(m => new MatchDto
                    {
                        Player1Id = m.Player1Id,
                        Player2Id = m.Player2Id,
                        WinnerId = m.WinnerId
                    })]
                })]
            };
        }
    }
}
