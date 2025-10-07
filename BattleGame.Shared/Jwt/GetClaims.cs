namespace BattleGame.Shared.Jwt
{
    public class GetClaims(IHttpContextAccessor httpContextAccessor)
    {
        public Guid GetUserId()
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId is null ? Guid.Empty : Guid.Parse(userId);
        }
    }
}
