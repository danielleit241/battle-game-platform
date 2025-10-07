namespace BattleGame.MessageBus.Events
{
    public record GameCompeletedEvent(Guid GameId, Guid UserId, DateTime CompletedAt);
}
