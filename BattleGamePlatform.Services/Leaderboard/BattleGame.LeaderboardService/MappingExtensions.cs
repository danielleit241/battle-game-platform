using BattleGame.LeaderboardService.Dtos;
using BattleGame.LeaderboardService.Entities;
using BattleGame.MessageBus.Events;

namespace BattleGame.LeaderboardService
{
    public static class MappingExtensions
    {
        public static LeaderboardDto AsDto(this Leaderboard entity, string Username) => new LeaderboardDto
        (
            Id: entity.Id,
            UserId: entity.UserId,
            Username: Username,
            TotalScore: entity.TotalScore,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );

        public static Game AsEntity(this GameCreatedEvent @event) => new Game
        {
            Id = @event.GameId,
            GameName = @event.GameName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        public static Game AsEntity(this GameUpdatedEvent @event) => new Game
        {
            Id = @event.GameId,
            GameName = @event.GameName,
            UpdatedAt = DateTime.UtcNow
        };

        public static Leaderboard AsEntity(this MatchCompletedEvent @event) => new Leaderboard
        {
            Id = Guid.NewGuid(),
            UserId = @event.UserId,
            GameId = @event.GameId,
            TotalScore = @event.Score,
            CreatedAt = @event.CompletedAt,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
