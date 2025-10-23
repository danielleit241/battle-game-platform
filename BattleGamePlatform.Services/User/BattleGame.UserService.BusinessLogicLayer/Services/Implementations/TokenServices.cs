namespace BattleGame.UserService.BusinessLogicLayer.Services.Implementations
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = GetClaims(user);
            var creds = GetCredentials();
            var expires = GetExpries();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = creds
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role.Name)
                };
            return claims;
        }

        private SigningCredentials GetCredentials()
        {
            var secretKey = _configuration["Authentication:Jwt:Key"];
            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 16)
                throw new ArgumentException("JWT secret key must be at least 16 characters.");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        }

        private DateTime GetExpries() => DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:Jwt:ExpiresMinutes"] ?? "30"));
    }
}
