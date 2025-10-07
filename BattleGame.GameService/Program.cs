var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGameApi();

await app.MigrateDbContextAsync<GameServiceDbContext>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
