var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddMongoDb(Const.LeaderboardDatabase);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<MatchCompletedEventConsumer>();
builder.Services.AddScoped<GameCreatedEventConsumer>();
builder.Services.AddScoped<UserCreatedEventConsumer>();

builder.Services.AddHttpClient<UserClient>(_ =>
{
    _.BaseAddress = new Uri(builder.Configuration["Clients:Gateway"] ?? builder.Configuration["Clients:UserClient"] ?? throw new Exception("User client is empty"));
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<ILeaderboardServices, LeaderboardServices>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString(Const.Redis) ?? throw new Exception("Redis connection string is empty"))
);
builder.Services.AddScoped<RedisLeaderboardCache>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapLeaderboardApi();

app.Run();
