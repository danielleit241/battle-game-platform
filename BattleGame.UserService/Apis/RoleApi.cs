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

            return group;
        }

        private static async Task<Results<Ok<ApiResponse<RoleDto>>, NotFound>> CreateRole(
            [FromBody] CreateRoleDto dto,
            IRoleRepository repository)
        {
            if (dto is null)
            {
                return TypedResults.NotFound();
            }

            var role = dto.AsRole();
            await repository.AddAsync(role);

            var responseDto = role.AsRoleDto();
            return TypedResults.Ok(ApiResponse<RoleDto>.SuccessResponse(responseDto, metadata: DateTime.UtcNow));
        }

        private static async Task<Results<Ok<ApiResponse<IEnumerable<RoleDto>>>, NotFound>> GetAllRoles(IRoleRepository repository)
        {
            var roles = await repository.GetAllAsync();
            if (roles is null)
            {
                return TypedResults.NotFound();
            }
            var responseDtos = roles.Select(r => r.AsRoleDto());
            return TypedResults.Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(responseDtos));
        }
    }
}
