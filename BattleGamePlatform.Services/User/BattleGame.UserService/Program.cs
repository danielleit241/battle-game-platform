var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
await app.MigrateDbContextAsync<UserDbContext>();

app.UseAuthentication();
app.UseAuthorization();

app.MapRoleApi();
app.MapUserApi();

app.Run();
