using BattleGame.Shared.Common;

namespace BattleGame.UserService.BusinessLogicLayer.Services.Abstractions
{
    public interface IRoleServices
    {
        Task<ApiResponse<IReadOnlyCollection<RoleDto>>> GetAllRolesAsync();
        Task<ApiResponse<RoleDto>> CreateRoleAsync(CreateRoleDto dto);
        Task<ApiResponse<RoleDto>> DeleteRoleAsync(Guid id);
    }
}
