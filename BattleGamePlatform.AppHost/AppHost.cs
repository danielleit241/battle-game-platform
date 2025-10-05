var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithHostPort(5432)
    .WithPgAdmin(pgAdmin =>
    {
        pgAdmin.WithHostPort(5050);
    })
    .WithDataVolume();

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

var userservice = builder.AddProject<Projects.BattleGame_UserService>("battlegame-userservice")
    .WithReference(userdb)
    .WaitFor(postgres);

var gameservice = builder.AddProject<Projects.BattleGame_GameService>("battlegame-gameservice")
    .WithReference(gamedb)
    .WithReference(rabbitMq)
    .WaitFor(postgres);

var matchservice = builder.AddProject<Projects.BattleGame_MatchService>("battlegame-matchservice")
    .WithReference(matchdb)
    .WithReference(rabbitMq)
    .WaitFor(mongo);

var leaderboardservice = builder.AddProject<Projects.BattleGame_LeaderboardService>("battlegame-leaderboardservice")
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WaitFor(redis);

var gateway = builder.AddYarp("gateway")
    .WithHostPort(8080)
    .WithConfiguration(cYarp =>
    {
        cYarp.AddRoute("/api/v1/users/{**catch-all}", userservice);
        cYarp.AddRoute("/api/v1/games/{**catch-all}", gameservice);
        cYarp.AddRoute("/api/v1/matchs/{**catch-all}", matchservice);
        cYarp.AddRoute("/api/v1/leaderboard/{**catch-all}", leaderboardservice);
    })
    .WaitFor(userservice)
    .WaitFor(gameservice)
    .WaitFor(matchservice)
    .WaitFor(leaderboardservice);

builder.Build().Run();
