namespace BattleGame.UserService.BusinessLogicLayer.Services.Implementations
{
    public class RoleServices(IRoleRepository repository) : IRoleServices
    {
        public async Task<ApiResponse<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            var roles = await repository.GetAllAsync();
            if (roles.Any(r => r.Name == dto.Name))
            {
                return ApiResponse<RoleDto>.FailureResponse("Role with the same name already exists");
            }
            var role = dto.AsRole();
            await repository.AddAsync(role);
            var roleDto = role.AsRoleDto();
            return ApiResponse<RoleDto>.SuccessResponse(roleDto, "Role created successfully");
        }

        public async Task<ApiResponse<RoleDto>> DeleteRoleAsync(Guid id)
        {
            var role = await repository.GetAsync(r => r.Id == id);
            if (role is null)
            {
                return ApiResponse<RoleDto>.FailureResponse("Role not found");
            }
            await repository.DeleteAsync(role);
            var roleDto = role.AsRoleDto();
            return ApiResponse<RoleDto>.SuccessResponse(roleDto, "Role deleted successfully");
        }

        public async Task<ApiResponse<IReadOnlyCollection<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await repository.GetAllAsync();
            if (roles is null || !roles.Any())
            {
                return ApiResponse<IReadOnlyCollection<RoleDto>>.FailureResponse("No roles found");
            }
            var roleDtos = roles.Select(r => r.AsRoleDto()).ToList().AsReadOnly();
            return ApiResponse<IReadOnlyCollection<RoleDto>>.SuccessResponse(roleDtos, "Roles retrieved successfully");
        }
    }
}
