namespace BattleGame.TestHelpers.TestHelpers.Dtos
{
    public record LoginDto(string Username, string Password);
    public record TokenDto(string Token);
    public record CreateUserDto(string Username, string? Email, string Password, Guid RoleId);
    public record UserDto(Guid Id, string Username, string? Email, Guid RoleId, string RoleName);
    public record UpdateUserDto(string? Username, string? Email, string? Password, Guid? RoleId);
    public record DeleteUserDto(Guid Id);
}
