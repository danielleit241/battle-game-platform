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
            return group;
        }
    }
}
