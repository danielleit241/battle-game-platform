namespace BattleGame.MessageBus.Events
{
    public record GameCompletedEvent(Guid GameId, Guid UserId, DateTime CompletedAt);
}
