using BattleGame.UserService.Api.Apis;
using BattleGame.UserService.Api.Bootstrapping;
using BattleGamePlatform.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.Services.AddJwtConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapRoleApi();
app.MapUserApi();

await app.MigrateDbContextAsync<UserDbContext>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
