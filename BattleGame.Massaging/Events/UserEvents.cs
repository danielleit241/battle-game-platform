namespace BattleGame.MessageBus.Events
{
    public record UserCreatedEvent(Guid Id, string Username, string? Email, Guid RoleId, DateTime CreatedAt);
}
