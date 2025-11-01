using BattleGame.GameService.Search;
using BattleGame.MessageBus.Events;
using Elastic.Clients.Elasticsearch;
using MassTransit;

namespace BattleGame.GameService.SearchService.EventHandlers
{
    public class GameDeletedEventHandler : IConsumer<GameDeletedEvent>
    {
        private readonly ILogger<GameDeletedEventHandler> _logger;
        private readonly ElasticsearchClient _elasticsearchClient;

        public GameDeletedEventHandler(ILogger<GameDeletedEventHandler> logger, ElasticsearchClient elasticsearchClient)
        {
            _logger = logger;
            _elasticsearchClient = elasticsearchClient;
        }
        public async Task Consume(ConsumeContext<GameDeletedEvent> context)
        {
            try
            {
                var message = context.Message;
                _logger.LogInformation("Received GameDeletedEvent for GameId: {GameId}", message.GameId);

                await _elasticsearchClient.DeleteAsync(message.GameId.ToString(), d => d.Index(nameof(GameIndexDocument)));
                _logger.LogInformation("Deleted GameIndexDocument for GameId: {GameId} from Elasticsearch", message.GameId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GameDeletedEvent for GameId: {GameId}", context.Message.GameId);
                throw;
            }
        }
    }
}
