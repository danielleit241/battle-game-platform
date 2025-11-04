using BattleGame.MessageBus;
using BattleGame.Shared.Common;
using BattleGame.Shared.Database;
using BattleGame.Shared.Jwt;
using BattleGame.TournamentService.CQRSServices.Tournament.Command;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGamePlatform.DatabaseMigrationHelpers;
using BattleGamePlatform.ServiceDefaults;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDb<TournamentWriteDbContext>(Const.TournamentDatabase + "Write");
builder.AddMongoDb(Const.TournamentDatabase + "Read");

builder.AddJwtConfiguration(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddMediatR(typeof(CreateTournamentHandler).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.MigrateDbContextAsync<TournamentWriteDbContext>();

app.UseHttpsRedirection();

app.Run();
