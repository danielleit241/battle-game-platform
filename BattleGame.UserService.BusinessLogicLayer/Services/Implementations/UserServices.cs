using BattleGame.Shared.Common;

namespace BattleGame.UserService.BusinessLogicLayer.Services.Implementations
{
    public class UserServices(
        IUserRepository repository,
        IRoleRepository roleRepository,
        ITokenServices tokenService,
        IMessagePublisher publisher) : IUserServices
    {
        public async Task<ApiResponse<IReadOnlyCollection<UserDto>>> GetAllUsersAsync()
        {
            var users = await repository.GetAllUserIncludeRoleAsync();
            if (users is null || !users.Any())
                return ApiResponse<IReadOnlyCollection<UserDto>>.FailureResponse("No users found");

            var userDtos = users.Select(u => u.AsUserDto()).ToList().AsReadOnly();
            return ApiResponse<IReadOnlyCollection<UserDto>>.SuccessResponse(userDtos, "Users retrieved successfully");
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id)
        {
            var user = await repository.GetUserIncludeRoleAsync(id);
            if (user is null)
                return ApiResponse<UserDto>.FailureResponse("No users found");

            var userDto = user.AsUserDto();
            return ApiResponse<UserDto>.SuccessResponse(userDto, "User retrieved successfully");
        }

        public async Task<ApiResponse<TokenDto>> LoginUserAsync(LoginDto dto)
        {
            var user = await repository.GetAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return ApiResponse<TokenDto>.FailureResponse("Username does not exist.");
            }
            var passwordValid = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordValid == PasswordVerificationResult.Failed)
            {
                return ApiResponse<TokenDto>.FailureResponse("Password is incorrect.");
            }
            var userWithRole = await repository.GetUserIncludeRoleAsync(user.Id);
            var token = tokenService.GenerateAccessToken(userWithRole!);
            return ApiResponse<TokenDto>.SuccessResponse(new TokenDto(token), "Login successful");
        }

        public async Task<ApiResponse<UserDto>> RegisterUserAsync(CreateUserDto dto)
        {
            var role = await roleRepository.GetAsync(r => r.Id == dto.RoleId);
            if (role is null)
                return ApiResponse<UserDto>.FailureResponse("Role does not exist");

            var existingUser = await repository.GetAsync(u => u.Username == dto.Username);
            if (existingUser is not null)
                return ApiResponse<UserDto>.FailureResponse("Username already exists");

            var user = dto.AsUser();
            await repository.AddAsync(user);

            _ = publisher.Publish("user.created", new UserCreatedEvent
            (
                Id: user.Id,
                Username: user.Username,
                Email: user.Email,
                RoleId: user.RoleId,
                CreatedAt: user.CreatedAt
            ));

            var userDto = user.AsUserDto();
            return ApiResponse<UserDto>.SuccessResponse(userDto, "User registered successfully");
        }
    }
}
