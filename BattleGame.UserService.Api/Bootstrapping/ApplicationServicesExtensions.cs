using BattleGame.Shared.Common;
using BattleGamePlatform.ServiceDefaults;

namespace BattleGame.UserService.Api.Bootstrapping
{
    public static class ApplicationServicesExtensions
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.Services.AddOpenApi();

            builder.AddNpgsqlDbContext<UserDbContext>(
                Const.UserDatabase,
                configureDbContextOptions: options =>
                {
                    options.UseNpgsql(builder => builder.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));
                }
            );

            builder.Services.AddSingleton<IMessagePublisher>(sp =>
                new RabbitMqPublisher(
                    connectionString: builder.Configuration.GetConnectionString("RabbitMq")
                            ?? throw new InvalidOperationException("RabbitMq connection string not configured"),
                    logger: sp.GetRequiredService<ILogger<RabbitMqPublisher>>()
                    )
            );

            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();

            return builder;
        }

    }
}
