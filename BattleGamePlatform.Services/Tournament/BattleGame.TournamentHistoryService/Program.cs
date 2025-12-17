using BattleGame.MessageBus;
using BattleGame.Shared;
using BattleGame.Shared.Common;
using BattleGame.TournamentHistoryService.Apis;
using BattleGame.TournamentHistoryService.CQRSServices.Tournament.Query;
using BattleGame.TournamentHistoryService.Repositories;
using BattleGame.TournamentHistoryService.Repositories.Interfaces;
using BattleGamePlatform.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.AddServiceDefaults();
builder.AddRateLimit();
builder.AddMongoDb(Const.TournamentDatabase + "Read");
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentParticipantRepository, TournamentParticipantRepository>();
builder.Services.AddScoped<ITournamentRoundRepository, TournamentRoundRepository>();
builder.Services.AddScoped<ITournamentMatchRepository, TournamentMatchRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllTournamentQuery).Assembly));

builder.Services.AddOpenApi();
var app = builder.Build();
app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapTournamentApis();
app.Run();