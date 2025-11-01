namespace BattleGame.GameService.SearchService
{
    public class ApiServices
    {
        public ElasticsearchClient ElasticsearchClient { get; init; }
        public CancellationToken CancellationToken { get; init; }

        public ApiServices(ElasticsearchClient elasticsearchClient, CancellationToken cancellationToken)
        {
            ElasticsearchClient = elasticsearchClient;
            CancellationToken = cancellationToken;
        }
    }
}
