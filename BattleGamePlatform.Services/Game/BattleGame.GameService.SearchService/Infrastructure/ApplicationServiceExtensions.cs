namespace BattleGame.GameService.SearchService.Infrastructure
{
    public static class ApplicationServiceExtensions
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();

            builder.Services.AddOpenApi();
            builder.AddElasticsearchClient(connectionName: Const.Elasticsearch, configureClientSettings: settings =>
            {
                settings.DefaultMappingFor<GameIndexDocument>(m => m
                    .IndexName(nameof(GameIndexDocument)));
            });
            builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
            builder.Services.AddScoped<GameEsMapper>();

            return builder;
        }
    }
}
