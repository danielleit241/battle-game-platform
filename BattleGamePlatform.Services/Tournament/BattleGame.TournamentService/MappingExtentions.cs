using BattleGame.TournamentService.Dtos;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.IntergrationEvents;

namespace BattleGame.TournamentService
{
    public static class MappingExtentions
    {
        public static CreatedTournamentIntergrationEvent AsTournamentCreatedEvent(this Tournament tournament)
        {
            return new CreatedTournamentIntergrationEvent
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                MaxParticipants = tournament.MaxParticipants,
                GameId = tournament.GameId,
                Status = (int)tournament.Status,
                Format = (int)tournament.Format,
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
                CreatedAt = round.CreatedAt,
                UpdatedAt = round.UpdatedAt
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
                Status = TournamentStatus.Upcoming,
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
    }
}
