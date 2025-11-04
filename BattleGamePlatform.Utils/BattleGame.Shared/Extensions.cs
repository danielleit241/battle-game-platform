using BattleGame.Shared.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace BattleGame.Shared
{
    public static class Extensions
    {
        public static IHostApplicationBuilder AddRateLimit(this IHostApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(policyName: "Fixed", options =>
                {
                    options.PermitLimit = 100;
                    options.Window = TimeSpan.FromMinutes(1);
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 0;
                });
            });

            builder.Services.Configure<RateLimiterOptions>(options =>
            {
                options.RejectionStatusCode = 429;
            });

            return builder;
        }

        public static IHostApplicationBuilder AddJwtConfiguration(this IHostApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                });
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<GetClaims>();
            return builder;
        }

        public static IHostApplicationBuilder AddMongoDb(this IHostApplicationBuilder builder, string databaseName)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));

            builder.Services.AddSingleton<IMongoDatabase>(_ =>
            {
                var mongoConnectionString = builder.Configuration.GetConnectionString(databaseName);
                var client = new MongoClient(mongoConnectionString);
                return client.GetDatabase(databaseName);
            });

            return builder;
        }

        public static IHostApplicationBuilder AddNpgsqlDb<TContext>(
            this IHostApplicationBuilder builder,
            string databaseName)
            where TContext : DbContext
        {
            builder.AddNpgsqlDbContext<TContext>(databaseName, configureDbContextOptions: options =>
            {
                options.UseNpgsql(builder => builder.MigrationsAssembly(typeof(TContext).Assembly.FullName));
            });

            return builder;
        }
    }
}
