using BattleGame.TournamentService.Entities;

namespace BattleGame.TournamentService.Dtos
{
    public record TournamentDto(
            Guid Id,
            string Name,
            string Description,
            int MaxParticipants,
            Guid GameId,
            TournamentFormat Format,
            DateTime StartDate,
            DateTime EndDate,
            DateTime CreatedAt,
            DateTime UpdatedAt
        );

    public record CreateTournamentDto(
            string Name,
            string Description,
            int MaxParticipants,
            Guid GameId,
            TournamentFormat Format,
            DateTime StartDate,
            DateTime EndDate
        );

    public record RegisterTournamentDto(
            Guid Id,
            string ParticipantName,
            Guid TournamentId
        );
}
