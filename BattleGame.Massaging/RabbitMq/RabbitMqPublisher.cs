namespace BattleGame.MessageBus.RabbitMq
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqPublisher> _logger;

        public RabbitMqPublisher(string connectionString, ILogger<RabbitMqPublisher> logger)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            _logger = logger;
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task Publish<T>(string queueName, T message) where T : class
        {
            _logger.LogWarning("Publishing message to queue {QueueName}: {Message}", queueName, message);
            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _logger.LogWarning("Queue {QueueName} declared successfully", queueName);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _logger.LogWarning("Message serialized to JSON: {Json}", json);
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                body: body);
            _logger.LogWarning("Message published to queue {QueueName}", queueName);
        }
    }
}
