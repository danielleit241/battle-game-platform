var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

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
