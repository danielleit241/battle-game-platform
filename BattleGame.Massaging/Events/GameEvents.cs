namespace BattleGame.MessageBus.Events
{
    public record GameCompletedEvent(Guid GameId, Guid UserId, DateTime CompletedAt);
    public record GameCreatedEvent(Guid GameId, string GameName, DateTime CreatedAt);
    public record GameUpdatedEvent(Guid GameId, string GameName, DateTime UpdatedAt);
    public record GameDeletedEvent(Guid GameId, DateTime DeletedAt);
}
