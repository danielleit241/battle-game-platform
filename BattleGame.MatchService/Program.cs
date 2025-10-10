using BattleGame.MatchService.Api;
using BattleGame.MatchService.Clients;
using BattleGame.MatchService.Consumers;
using BattleGame.MatchService.Repositories;
using BattleGame.MatchService.Services;
using BattleGame.MessageBus;
using BattleGame.Shared.Common;
using BattleGame.Shared.Jwt;
using BattleGamePlatform.ServiceDefaults;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddJwtConfiguration(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<GameCompletedConsumer>();
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));

builder.Services.AddSingleton<IMongoDatabase>(_ =>
{
    var mongoConnectionString = builder.Configuration.GetConnectionString(Const.MatchDatabase);
    var client = new MongoClient(mongoConnectionString);
    return client.GetDatabase(Const.MatchDatabase);
});

builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchLogService, MatchLogService>();
builder.Services.AddHttpClient<UserClient>(_ =>
{
    _.BaseAddress = new Uri(builder.Configuration["Clients:Gateway"] ?? throw new Exception("User client is empty"));
});


builder.Services.AddHttpClient<GameClient>(_ =>
{
    _.BaseAddress = new Uri(builder.Configuration["Clients:Gateway"] ?? throw new Exception("Game client is empty"));
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapMatchApi();

app.Run();
