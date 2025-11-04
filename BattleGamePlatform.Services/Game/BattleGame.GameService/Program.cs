var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.MigrateDbContextAsync<GameServiceDbContext>();
app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapGameApi();

app.Run();
