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

        public static RoleDto AsDto(this Role role)
            => new(role.Id, role.Name);
    }
}
