using BattleGame.Shared.Common;

namespace BattleGame.UserService.BusinessLogicLayer.Services.Abstractions
{
    public interface IUserServices
    {
        Task<ApiResponse<UserDto>> RegisterUserAsync(CreateUserDto dto);
        Task<ApiResponse<TokenDto>> LoginUserAsync(LoginDto dto);
        Task<ApiResponse<IReadOnlyCollection<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id);
    }
}
