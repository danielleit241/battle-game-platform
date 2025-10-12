using BattleGame.LeaderboardService.Repositories;
using BattleGame.MessageBus.Events;
using MassTransit;

namespace BattleGame.LeaderboardService.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventConsumer> _logger;
        private readonly IUserRepository userRepository;
        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger, IUserRepository userRepository)
        {
            _logger = logger;
            this.userRepository = userRepository;
        }
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Received UserCreatedEvent: {Message}", message);
            var user = new Entities.User
            {
                Id = message.Id,
                Username = message.Username,
                CreatedAt = message.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
            return userRepository.AddAsync(user);
        }
    }
}
