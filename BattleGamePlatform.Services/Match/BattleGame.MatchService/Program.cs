var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

builder.AddMongoDb(Const.MatchDatabase);
builder.AddJwtConfiguration(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<GameCompletedEventConsumer>();

builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchLogService, MatchLogService>();
builder.Services.AddHttpClient<UserClient>(_ =>
{
    _.BaseAddress = new Uri(builder.Configuration["Clients:Gateway"] ?? builder.Configuration["Clients:UserClient"] ?? throw new Exception("User client is empty"));
});


builder.Services.AddHttpClient<GameClient>(_ =>
{
    _.BaseAddress = new Uri(builder.Configuration["Clients:Gateway"] ?? builder.Configuration["Clients:GameClient"] ?? throw new Exception("Game client is empty"));
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapMatchApi();

app.Run();
