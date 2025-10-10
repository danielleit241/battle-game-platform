using BattleGame.UserService.Api.Apis;
using BattleGame.UserService.Api.Bootstrapping;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.MigrateDbContextAsync<UserDbContext>();

app.UseAuthentication();
app.UseAuthorization();

app.MapRoleApi();
app.MapUserApi();

app.Run();
