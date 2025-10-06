using BattleGame.MessageBus.Abstractions;
using BattleGame.MessageBus.RabbitMq;
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

        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretKey = configuration["Authentication:Jwt:Key"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.ASCII.GetBytes(secretKey)),
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogWarning("JWT Authentication failed: {Exception}", context.Exception?.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogDebug("JWT Token validated for user: {UserId}",
                                context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
            return services;
        }
    }
}
