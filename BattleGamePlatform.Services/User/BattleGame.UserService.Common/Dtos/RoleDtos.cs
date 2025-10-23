namespace BattleGame.UserService.Common.Dtos
{
    public record RoleDto(Guid Id, string Name);
    public record CreateRoleDto(string Name);
    public record UpdateRoleDto(string Name);
    public record DeleteRoleDto(Guid Id);
}
