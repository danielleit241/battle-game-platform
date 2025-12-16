using BattleGame.MessageBus;
using BattleGame.Shared;
using BattleGame.Shared.Common;
using BattleGame.TournamentService.Apis;
using BattleGame.TournamentService.CQRSServices.Tournament.Command;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGamePlatform.DatabaseMigrationHelpers;
using BattleGamePlatform.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRateLimit();
builder.AddNpgsqlDb<TournamentWriteDbContext>(Const.TournamentDatabase + "Write");
builder.AddJwtConfiguration(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterTournamentHandler).Assembly));

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.MigrateDbContextAsync<TournamentWriteDbContext>();
app.UseMiddleware<GlobalExceptionsMiddleware>();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapTournamentApi();
app.MapMatchApi();
app.Run();
