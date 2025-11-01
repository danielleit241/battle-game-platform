using BattleGame.GameService.Search;
using BattleGame.MessageBus.Events;
using Elastic.Clients.Elasticsearch;
using MassTransit;

namespace BattleGame.GameService.SearchService.EventHandlers
{
    public class GameUpdatedEventHandler : IConsumer<GameUpdatedEvent>
    {
        private readonly ILogger<GameUpdatedEventHandler> _logger;
        private readonly ElasticsearchClient _elasticSearchClient;
        public GameUpdatedEventHandler(ILogger<GameUpdatedEventHandler> logger, ElasticsearchClient elasticSearchClient)
        {
            _logger = logger;
            _elasticSearchClient = elasticSearchClient;
        }
        public async Task Consume(ConsumeContext<GameUpdatedEvent> context)
        {
            try
            {

                var message = context.Message;
                _logger.LogInformation("Received GameUpdatedEvent for GameId: {GameId}", message.GameId);

                var request = new UpdateRequest<GameIndexDocument, object>(nameof(GameIndexDocument).ToLower(), message.GameId.ToString())
                {
                    Doc = new
                    {
                        GameName = message.GameName,
                        GameDes = message.GameDes,
                        MaxPlayers = message.MaxPlayers,
                        UpdatedAt = message.UpdatedAt
                    }
                };

                if (request.Doc != null)
                {
                    var response = await _elasticSearchClient.UpdateAsync(request);
                    if (response.IsValidResponse)
                    {
                        _logger.LogInformation("Successfully updated GameIndexDocument for GameId: {GameId}", message.GameId);
                    }
                    else
                    {
                        _logger.LogError("Failed to update GameIndexDocument for GameId: {GameId}. Error: {Error}", message.GameId, response.ElasticsearchServerError?.Error.Reason);
                    }
                }
            }
            catch
            {
                _logger.LogError("Exception occurred while processing GameUpdatedEvent for GameId: {GameId}", context.Message.GameId);
                throw;
            }
        }
    }
}
