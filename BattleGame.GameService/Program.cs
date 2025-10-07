using BattleGame.GameService.Apis;
using BattleGame.GameService.BusinessLogicLayer.Services.Abstractions;
using BattleGame.GameService.BusinessLogicLayer.Services.Implementations;
using BattleGame.GameService.DataAccessLayer.Infrastructure.Data;
using BattleGame.GameService.DataAccessLayer.Repositories.Abstractions;
using BattleGame.GameService.DataAccessLayer.Repositories.Implementations;
using BattleGame.MessageBus.Abstractions;
using BattleGame.MessageBus.RabbitMq;
using BattleGame.Shared.Common;
using BattleGame.Shared.Jwt;
using BattleGamePlatform.DatabaseMigrationHelpers;
using BattleGamePlatform.ServiceDefaults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<GameServiceDbContext>(
    Const.GameDatabase,
    configureDbContextOptions: options =>
    {
        options.UseNpgsql(builder => builder.MigrationsAssembly(typeof(GameServiceDbContext).Assembly.FullName));
    }
);

builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GetClaims>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameServices, GameServices>();
builder.Services.AddSingleton<IMessagePublisher>(sp =>
    new RabbitMqPublisher(
        connectionString: builder.Configuration.GetConnectionString("RabbitMq")
                ?? throw new InvalidOperationException("RabbitMq connection string not configured"),
        logger: sp.GetRequiredService<ILogger<RabbitMqPublisher>>()
        )
);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGameApi();

await app.MigrateDbContextAsync<GameServiceDbContext>();

app.Run();
