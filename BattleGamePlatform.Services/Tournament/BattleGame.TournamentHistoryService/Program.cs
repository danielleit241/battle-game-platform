using BattleGame.MessageBus;
using BattleGame.Shared;
using BattleGame.Shared.Common;
using BattleGamePlatform.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.AddServiceDefaults();
builder.AddRateLimit();
builder.Services.AddOpenApi();
builder.AddMongoDb(Const.TournamentDatabase + "Read");
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);


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

app.Run();