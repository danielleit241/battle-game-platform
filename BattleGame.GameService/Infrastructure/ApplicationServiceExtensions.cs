namespace BattleGame.GameService.Infrastructure
{
    public static class ApplicationServiceExtensions
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.AddNpgsqlDb<GameServiceDbContext>(Const.GameDatabase);
            builder.AddJwtConfiguration(builder.Configuration);
            builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IGameServices, GameServices>();
            builder.Services.AddOpenApi();

            return builder;
        }
    }
}
