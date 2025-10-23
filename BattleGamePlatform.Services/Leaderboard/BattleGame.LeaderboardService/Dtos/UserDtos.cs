namespace BattleGame.MatchService.Dtos
{
    public record UserDto(Guid Id, string Username, string? Email, Guid RoleId, string RoleName);
}
