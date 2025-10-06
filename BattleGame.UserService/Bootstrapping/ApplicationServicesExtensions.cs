namespace BattleGame.UserService.Bootstrapping
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

            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            return builder;
        }
    }
}
