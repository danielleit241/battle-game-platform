namespace BattleGamePlatform.AppHost.Bootstrapping
{
    public static class ExternalServiceRegistrationExtensions
    {
        private const bool GatewayDangerousAcceptAnyServerCertificate = true;

        public static IDistributedApplicationBuilder AddApplicationServices(this IDistributedApplicationBuilder builder)
        {
            var postgres = builder.AddPostgres("postgres")
                .WithContainerName("postgres-dev")
                .WithImageTag("17")
                .WithHostPort(5432)
                .WithPgWeb(pgWeb =>
                {
                    pgWeb.WithHostPort(5050);
                    pgWeb.WithContainerName("pgweb-dev");
                })
                .WithDataVolume();

            var mongo = builder.AddMongoDB("mongo")
                .WithContainerName("mongo-dev")
                .WithImageTag("latest")
                .WithDataVolume();

            var redis = builder.AddRedis("redis")
                .WithContainerName("redis-dev")
                .WithImageTag("latest")
                .WithHostPort(6379)
                .WithDataVolume();

            var rabbitMq = builder.AddRabbitMQ("rabbitmq")
                .WithContainerName("rabbitmq-dev")
                .WithImageTag("3-management")
                .WithManagementPlugin(15672)
                .WithDataVolume();

            var elasticSearch = builder.AddElasticsearch("elasticsearch")
                .WithContainerName("elasticsearch-dev")
                .WithContainerRuntimeArgs("--memory=512m");

            var userdb = postgres.AddDatabase("userservice", "userdb");
            var gamedb = postgres.AddDatabase("gameservice", "gamedb");
            var matchdb = mongo.AddDatabase("matchservice", "matchdb");
            var leaderboarddb = mongo.AddDatabase("leaderboardservice", "leaderboarddb");
            var tournamentWriteDb = postgres.AddDatabase("tournamentservice-write", "tournamentwritedb");
            var tournamentReadDb = mongo.AddDatabase("tournamentservice-read", "tournamentreaddb");

            var userservice = builder.AddProject<Projects.BattleGame_UserService>("battlegame-userservice")
                .WithReference(userdb)
                .WithReference(rabbitMq)
                .WaitFor(postgres)
                .WaitFor(rabbitMq);

            var gameservice = builder.AddProject<Projects.BattleGame_GameService>("battlegame-gameservice")
                .WithReference(gamedb)
                .WithReference(rabbitMq)
                .WaitFor(postgres)
                .WaitFor(rabbitMq);

            var gameSearchService = builder.AddProject<Projects.BattleGame_GameService_SearchService>("battlegame-gameservice-searchservice")
                .WithReference(rabbitMq)
                .WithReference(elasticSearch)
                .WaitFor(rabbitMq)
                .WaitFor(elasticSearch);

            var matchservice = builder.AddProject<Projects.BattleGame_MatchService>("battlegame-matchservice")
                .WithReference(matchdb)
                .WithReference(rabbitMq)
                .WaitFor(mongo)
                .WaitFor(rabbitMq);

            var leaderboardservice = builder.AddProject<Projects.BattleGame_LeaderboardService>("battlegame-leaderboardservice")
                .WithReference(leaderboarddb)
                .WithReference(redis)
                .WithReference(rabbitMq)
                .WaitFor(mongo)
                .WaitFor(redis)
                .WaitFor(rabbitMq);

            var tournamentService = builder.AddProject<Projects.BattleGame_TournamentService>("battlegame-tournamentservice")
                .WithReference(tournamentWriteDb)
                .WithReference(tournamentReadDb)
                .WithReference(rabbitMq)
                .WaitFor(postgres)
                .WaitFor(mongo)
                .WaitFor(rabbitMq);

            var gateway = builder.AddYarp("gateway")
                .WithContainerName("gateway-dev")
                .WithHostPort(8080)
                .WithConfiguration(yarp =>
                {
                    yarp.AddRoute("/api/v1/users/{**catch-all}", userservice);
                    yarp.AddRoute("/api/v1/games/{**catch-all}", gameservice);
                    yarp.AddRoute("/api/v1/games/{**catch-all}", gameSearchService);
                    yarp.AddRoute("/api/v1/matches/{**catch-all}", matchservice);
                    yarp.AddRoute("/api/v1/leaderboards/{**catch-all}", leaderboardservice);
                    yarp.AddRoute("/api/v1/tournaments/{**catch-all}", tournamentService);
                })
                .WaitFor(userservice)
                .WaitFor(gameservice)
                .WaitFor(matchservice)
                .WaitFor(leaderboardservice)
                .WaitFor(tournamentService);

            var scalar = builder.AddScalarApiReference()
                .WithContainerName("scalar-dev")
                .WithApiReference(userservice)
                .WithApiReference(gameservice).WithApiReference(gameSearchService)
                .WithApiReference(matchservice)
                .WithApiReference(leaderboardservice)
                .WithApiReference(tournamentService)
                .WaitFor(gateway);

            return builder;
        }

        private static YarpRoute AddRoute(this IYarpConfigurationBuilder yarp, string path, IResourceBuilder<ProjectResource> resource)
        {
            var serviceCluster = yarp.AddCluster(resource).WithHttpClientConfig(
                new Yarp.ReverseProxy.Configuration.HttpClientConfig() { DangerousAcceptAnyServerCertificate = GatewayDangerousAcceptAnyServerCertificate }
                );
            return yarp.AddRoute(path, serviceCluster);
        }
    }
}