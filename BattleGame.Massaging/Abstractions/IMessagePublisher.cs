namespace BattleGame.MessageBus.Abstractions
{
    public interface IMessagePublisher
    {
        Task Publish<T>(string queueName, T message) where T : class;
    }
}
