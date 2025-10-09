namespace BattleGame.Shared.Jwt
{
    public static class Extensions
    {
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
    }
}
