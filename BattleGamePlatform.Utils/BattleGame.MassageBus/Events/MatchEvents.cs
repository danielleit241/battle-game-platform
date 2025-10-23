namespace BattleGame.MessageBus.Events
{
    public record MatchCompletedEvent(Guid MatchId, Guid UserId, Guid GameId, int Score, DateTime CompletedAt);
}
