using BattleGame.MessageBus.Abstractions;

namespace BattleGame.MessageBus.RabbitMq
{
    public class RabbitMqSubscriber : IMessageSubscriber
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqSubscriber> _logger;

        public RabbitMqSubscriber(string connectionString, ILogger<RabbitMqSubscriber> logger)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            _logger = logger;
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task Subscribe(string queueName, Action<string> onMessage)
        {
            _logger.LogWarning("Subscribing to queue {QueueName}", queueName);
            await using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _logger.LogWarning("Queue {QueueName} declared successfully", queueName);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                onMessage(message);
                await Task.Yield();
            };
            _logger.LogWarning("Consumer created for queue {QueueName}", queueName);
            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumerTag: "",
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer);
            _logger.LogWarning("Subscribed to queue {QueueName} successfully", queueName);
        }
    }
}
