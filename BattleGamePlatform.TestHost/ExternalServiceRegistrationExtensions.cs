using Aspire.Hosting.Yarp;

namespace BattleGamePlatform.TestHost
{
    public static class ExternalServiceRegistrationExtensions
    {
        private const bool GatewayDangerousAcceptAnyServerCertificate = true;

        public static IDistributedApplicationBuilder AddApplicationServices(this IDistributedApplicationBuilder builder)
        {
            var postgres = builder.AddPostgres("postgres")
                .WithImageTag("latest")
                .WithHostPort(5432)
                .WithPgWeb(pgWeb =>
                {
                    pgWeb.WithHostPort(5050);
                });

            var mongo = builder.AddMongoDB("mongo")
                .WithImageTag("latest");

            var redis = builder.AddRedis("redis")
                .WithImageTag("latest")
                .WithHostPort(6379);

            var rabbitMq = builder.AddRabbitMQ("rabbitmq")
                .WithImageTag("3-management")
                .WithManagementPlugin(15672);

            var userdb = postgres.AddDatabase("userservice", "userdb");
            var gamedb = postgres.AddDatabase("gameservice", "gamedb");
            var matchdb = mongo.AddDatabase("matchservice", "matchdb");
            var leaderboarddb = mongo.AddDatabase("leaderboardservice", "leaderboarddb");

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

            var gateway = builder.AddYarp("gateway")
                .WithHostPort(8080)
                .WithConfiguration(yarp =>
                {
                    yarp.AddRoute("/api/v1/users/{**catch-all}", userservice);
                    yarp.AddRoute("/api/v1/games/{**catch-all}", gameservice);
                    yarp.AddRoute("/api/v1/matches/{**catch-all}", matchservice);
                    yarp.AddRoute("/api/v1/leaderboards/{**catch-all}", leaderboardservice);
                })
                .WaitFor(userservice)
                .WaitFor(gameservice)
                .WaitFor(matchservice)
                .WaitFor(leaderboardservice);

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
