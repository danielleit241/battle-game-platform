namespace BattleGame.UserService.Dtos
{
    public record LoginDto(string Username, string Password);
    public record TokenDto(string Token);
    public record CreateUserDto(string UserName, string? Email, string Password, Guid RoleId);
    public record UserDto(Guid Id, string UserName, string? Email, Guid RoleId, string RoleName);
    public record UpdateUserDto(string? UserName, string? Email, string? Password, Guid? RoleId);
    public record DeleteUserDto(Guid Id);
}
