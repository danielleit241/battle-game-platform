using BattleGame.UserService.Common.Dtos;

namespace BattleGame.UserService.Api.Apis
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
            group.MapGet("", GetAllUsers).WithName("GetAllUsers").RequireAuthorization();
            group.MapGet("/{id:guid}", GetUserById).WithName("GetUserById").RequireAuthorization();
            group.MapPost("/register", RegisterUser).WithName("CreateUser");
            group.MapPost("/login", LoginUser).WithName("LoginUser");
            //group.MapPut("/{id:guid}", UpdateUser).WithName("UpdateUser");
            //group.MapDelete("/{id:guid}", DeleteUser).WithName("DeleteUser");

            return group;
        }

        private static async Task<IResult> LoginUser(LoginDto dto, IUserServices service)
        {
            var response = await service.LoginUserAsync(dto);
            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> GetAllUsers(IUserServices service)
        {
            var response = await service.GetAllUsersAsync();
            if (!response.IsSuccess)
            {
                return TypedResults.NotFound(response);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> GetUserById(Guid id, IUserServices service)
        {
            var response = await service.GetUserByIdAsync(id);
            if (!response.IsSuccess)
            {
                return TypedResults.NotFound(response);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> RegisterUser(CreateUserDto dto, IUserServices service)
        {
            var response = await service.RegisterUserAsync(dto);
            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }

    }
}
