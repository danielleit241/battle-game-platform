namespace BattleGame.UserService.Apis
{
    public static class RoleApi
    {
        public static IEndpointRouteBuilder MapRoleApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/users/roles")
                .MapRoleApi()
                .WithTags("Role Api");

            return builder;
        }

        public static RouteGroupBuilder MapRoleApi(this RouteGroupBuilder group)
        {
            group.MapGet("", GetAllRoles)
                .WithName("Get all roles");

            group.MapPost("", CreateRole)
                .WithName("Create role");

            group.MapDelete("/{id:guid}", DeleteRole)
                .WithName("Delete role");

            return group;
        }

        private static async Task<IResult> DeleteRole(Guid id, IRoleServices service)
        {
            var response = await service.DeleteRoleAsync(id);
            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> CreateRole(
            [FromBody] CreateRoleDto dto,
            IRoleServices service)
        {
            var response = await service.CreateRoleAsync(dto);
            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> GetAllRoles(IRoleServices service)
        {
            var response = await service.GetAllRolesAsync();
            if (!response.IsSuccess)
            {
                return TypedResults.NotFound(response);
            }
            return TypedResults.Ok(response);
        }
    }
}
