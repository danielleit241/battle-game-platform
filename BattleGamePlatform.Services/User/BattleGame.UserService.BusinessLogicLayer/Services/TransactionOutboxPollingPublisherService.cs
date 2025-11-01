namespace BattleGame.UserService.BusinessLogicLayer.Services
{
    public class TransactionOutboxPollingPublisherService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TransactionOutboxPollingPublisherService> _logger;
        private readonly ILogger<PollingPublisher> _publisherLogger;

        public TransactionOutboxPollingPublisherService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<TransactionOutboxPollingPublisherService> logger,
            ILogger<PollingPublisher> publisherLogger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _publisherLogger = publisherLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Transaction Outbox Polling Publisher Service.");
            using var scope = _serviceScopeFactory.CreateScope();
            var pollingPublisher = new PollingPublisher(
                _publisherLogger,
                scope.ServiceProvider.GetRequiredService<IPublishEndpoint>(),
                scope.ServiceProvider.GetRequiredService<ITransactionOutboxRepository>());

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await pollingPublisher.ProcessOutboxMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing TransactionOutboxService");
                }
            }

            _logger.LogInformation("Stopping Transaction Outbox Polling Publisher Service.");
        }
    }
}
