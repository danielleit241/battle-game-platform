using BattleGame.Shared;
using BattleGame.UserService.Apis;
using BattleGame.UserService.Infrastructure.Data;
using BattleGamePlatform.DatabaseMigrationHelpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<UserDbContext>(
    Const.UserDatabase,
    configureDbContextOptions: options =>
    {
        options.UseNpgsql(builder => builder.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));
    }
);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapRoleApi();
app.MapUserApi();

await app.MigrateDbContextAsync<UserDbContext>();

app.Run();
