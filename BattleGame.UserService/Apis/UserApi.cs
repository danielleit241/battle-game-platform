using Microsoft.AspNetCore.Identity;

namespace BattleGame.UserService.Apis
{
    public static class UserApi
    {
        public static IEndpointRouteBuilder MapUserApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/users")
                .MapUserApi()
                .WithTags("User Api");
            return builder;
        }
        public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder group)
        {
            group.MapGet("", GetAllUsers).WithName("GetAllUsers");
            group.MapGet("/{id:guid}", GetUserById).WithName("GetUserById");
            group.MapPost("/register", RegisterUser).WithName("CreateUser");
            group.MapPost("/login", LoginUser).WithName("LoginUser");
            //group.MapPut("/{id:guid}", UpdateUser).WithName("UpdateUser");
            //group.MapDelete("/{id:guid}", DeleteUser).WithName("DeleteUser");

            return group;
        }

        private static async Task<IResult> LoginUser(LoginDto dto, IUserRepository repository, TokenService tokenService)
        {
            var user = await repository.GetAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Results.NotFound(ApiResponse<TokenDto>.FailureResponse("Username does not exist."));
            }
            var passwordValid = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordValid == PasswordVerificationResult.Failed)
            {
                return Results.BadRequest(ApiResponse<TokenDto>.FailureResponse("Password is incorrect."));
            }
            var token = tokenService.GenerateAccessToken(user);
            return Results.Ok(ApiResponse<TokenDto>.SuccessResponse(new TokenDto(token), "Login successful"));
        }

        private static async Task<IResult> GetAllUsers(IUserRepository repository)
        {
            var users = await repository.GetAllUserIncludeRoleAsync();
            if (users is null)
            {
                return TypedResults.NotFound(ApiResponse<UserDto>.FailureResponse("User not found."));
            }
            var userDtos = users.Select(user => user.AsUserDto()).ToList().AsReadOnly();
            return TypedResults.Ok(ApiResponse<IReadOnlyCollection<UserDto>>.SuccessResponse(userDtos));
        }

        private static async Task<IResult> GetUserById(Guid id, IUserRepository repository)
        {
            var user = await repository.GetUserIncludeRoleAsync(id);
            if (user is null)
            {
                return TypedResults.NotFound(ApiResponse<UserDto>.FailureResponse("User not found."));
            }
            return TypedResults.Ok(ApiResponse<UserDto>.SuccessResponse(user.AsUserDto()));
        }

        private static async Task<IResult> RegisterUser(CreateUserDto dto, IUserRepository repository, IRoleRepository roleRepository)
        {
            var role = await roleRepository.GetAsync(dto.RoleId);
            if (role is null)
            {
                return Results.NotFound(ApiResponse<UserDto>.FailureResponse("Role not found"));
            }
            var user = dto.AsUser();
            await repository.AddAsync(user);
            return Results.Ok(ApiResponse<UserDto>.SuccessResponse(user.AsUserDto(), "User created successfully"));
        }

    }
}
