using Microsoft.AspNetCore.Identity;

namespace BattleGame.UserService.Bootstrapping
{
    public static class MappingExtensions
    {
        public static Role AsRole(this CreateRoleDto dto)
            => new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim().ToUpper(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

        public static RoleDto AsRoleDto(this Role role)
            => new(role.Id, role.Name);

        public static User AsUser(this CreateUserDto dto)
            => new()
            {
                Id = Guid.NewGuid(),
                Username = dto.UserName.Trim(),
                Email = dto.Email?.Trim().ToLower() ?? "",
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, dto.Password),
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

        public static UserDto AsUserDto(this User user)
            => new(user.Id, user.Username, user.Email, user.RoleId, user.Role.Name);
    }
}
