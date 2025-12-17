using BattleGame.TournamentService.Dtos;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.IntergrationEvents;

namespace BattleGame.TournamentService
{
    public static class MappingExtensions
    {
        public static MatchCompleteDto AsMatchCompleteDto(this TournamentMatch match)
        {
            return new MatchCompleteDto
            (
                MatchId: match.Id,
                TournamentId: match.Round.TournamentId,
                WinnerId: match.WinnerId ?? Guid.Empty,
                CompletedAt: DateTime.UtcNow
            );
        }

        public static CreatedTournamentIntergrationEvent AsTournamentCreatedEvent(this Tournament tournament)
        {
            return new CreatedTournamentIntergrationEvent
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                MaxParticipants = tournament.MaxParticipants,
                GameId = tournament.GameId,
                Status = (IntergrationEvents.TournamentStatus)tournament.Status,
                Format = (IntergrationEvents.TournamentFormat)tournament.Format,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate
            };
        }

        public static CreatedRoundIntergrationEvent AsRoundCreatedEvent(this TournamentRound round)
        {
            return new CreatedRoundIntergrationEvent
            {
                Id = round.Id,
                TournamentId = round.TournamentId,
                RoundNumber = round.RoundNumber,
                Status = (IntergrationEvents.TournamentRoundStatus)round.Status,
                CreatedAt = round.CreatedAt,
                UpdatedAt = round.UpdatedAt
            };
        }

        public static RoundCompletedIntergrationEvent AsRoundCompletedEvent(this TournamentRound round)
        {
            return new RoundCompletedIntergrationEvent
            {
                RoundId = round.Id,
                TournamentId = round.TournamentId,
                RoundNumber = round.RoundNumber,
                CompletedAt = DateTime.UtcNow
            };
        }

        public static CreatedParticipantIntergrationEvent AsParticipantCreatedEvent(this TournamentParticipant participant)
        {
            return new CreatedParticipantIntergrationEvent
            {
                Id = participant.Id,
                ParticipantName = participant.ParticipantName,
                TournamentId = participant.TournamentId,
                CreatedAt = participant.CreatedAt,
                UpdatedAt = participant.UpdatedAt
            };
        }

        public static CreatedMatchIntergrationEvent AsMatchCreatedEvent(this TournamentMatch match)
        {
            return new CreatedMatchIntergrationEvent
            {
                Id = match.Id,
                RoundId = match.RoundId,
                Player1Id = match.Player1Id,
                Player2Id = match.Player2Id,
                WinnerId = match.WinnerId,
                CreatedAt = match.CreatedAt,
                UpdatedAt = match.UpdatedAt
            };
        }

        public static MatchCompletedIntergrationEvent AsMatchCompletedEvent(this TournamentMatch match, Guid tournamentId)
        {
            return new MatchCompletedIntergrationEvent
            {
                MatchId = match.Id,
                RoundId = match.RoundId,
                TournamentId = tournamentId,
                WinnerId = match.WinnerId ?? Guid.Empty,
                CompletedAt = DateTime.UtcNow
            };
        }

        public static Tournament AsTournamentEntity(this CreateTournamentDto dto)
        {
            return new Tournament
            {
                Id = Guid.CreateVersion7(),
                Name = dto.Name,
                Description = dto.Description,
                MaxParticipants = dto.MaxParticipants,
                GameId = dto.GameId,
                Format = dto.Format,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = Entities.TournamentStatus.Upcoming,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        public static TournamentDto AsTournamentDto(this Tournament tournament)
        {
            return new TournamentDto
            (
                tournament.Id,
                tournament.Name,
                tournament.Description,
                tournament.MaxParticipants,
                tournament.GameId,
                tournament.Status,
                tournament.Format,
                tournament.StartDate,
                tournament.EndDate,
                tournament.CreatedAt,
                tournament.UpdatedAt
            );
        }

        public static TournamentParticipant AsParticipantEntity(this RegisterTournamentDto dto, Guid tournamentId)
        {
            return new TournamentParticipant
            {
                Id = dto.Id,
                ParticipantName = dto.ParticipantName,
                TournamentId = tournamentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
