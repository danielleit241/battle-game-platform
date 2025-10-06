namespace BattleGame.MessageBus.Abstractions
{
    public interface IMessageSubscriber
    {
        Task Subscribe(string queueName, Action<string> onMessage);
    }
}
