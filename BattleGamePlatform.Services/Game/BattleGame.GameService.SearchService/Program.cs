using BattleGame.GameService.Search;
using BattleGame.GameService.SearchService.Apis;
using BattleGame.MessageBus;
using BattleGame.Shared.Common;
using BattleGamePlatform.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.AddElasticsearchClient(connectionName: Const.Elasticsearch, configureClientSettings: settings =>
{
    settings.DefaultMappingFor<GameIndexDocument>(m => m
        .IndexName(nameof(GameIndexDocument)));
});
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<GameEsMapper>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapGameApi();

app.Run();