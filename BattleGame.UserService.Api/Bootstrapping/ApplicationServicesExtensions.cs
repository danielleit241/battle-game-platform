using BattleGame.MessageBus;
using BattleGame.Shared.Database;

namespace BattleGame.UserService.Api.Bootstrapping
{
    public static class ApplicationServicesExtensions
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.Services.AddOpenApi();
            builder.AddNpgsqlDb<UserDbContext>(Const.UserDatabase);
            builder.AddJwtConfiguration(builder.Configuration);
            builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();

            return builder;
        }

    }
}
