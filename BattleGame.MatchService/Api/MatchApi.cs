
using BattleGame.MatchService.Repositories;

namespace BattleGame.MatchService.Api
{
    public static class MatchApi
    {
        public static IEndpointRouteBuilder MapMatchApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/matches")
                .MapMatchApi()
                .WithTags("Matches Api");

            return builder;
        }

        public static RouteGroupBuilder MapMatchApi(this RouteGroupBuilder group)
        {
            group.MapGet("/{userId:guid}", GetMatchesByUserId)
                .WithName("User matches");

            return group;
        }

        private static async Task<IResult> GetMatchesByUserId(Guid userId, IMatchRepository repository)
        {
            return TypedResults.Ok();
        }
    }
}
