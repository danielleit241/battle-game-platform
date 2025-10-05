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
            return group;
        }
    }
}
