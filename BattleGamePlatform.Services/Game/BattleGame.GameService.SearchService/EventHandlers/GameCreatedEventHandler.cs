using BattleGame.GameService.Search;
using BattleGame.MessageBus.Events;
using Elastic.Clients.Elasticsearch;
using MassTransit;

public class GameCreatedConsumer : IConsumer<GameCreatedEvent>
{
    private readonly ILogger<GameCreatedConsumer> _logger;
    private readonly ElasticsearchClient _elasticsearchClient;
    private readonly GameEsMapper gameEsMapper;

    public GameCreatedConsumer(ILogger<GameCreatedConsumer> logger, ElasticsearchClient elasticsearchClient, GameEsMapper gameEsMapper)
    {
        _logger = logger;
        _elasticsearchClient = elasticsearchClient;
        this.gameEsMapper = gameEsMapper;
    }

    public async Task Consume(ConsumeContext<GameCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Game created event received: {GameName}", message.GameName);

        var document = gameEsMapper.Map(message);

        await _elasticsearchClient.IndexAsync(document, i => i.Index(nameof(GameIndexDocument).ToLower()));

        _logger.LogInformation("Game indexed to Elasticsearch with ID {Id}", message.GameId);
    }
}
