
namespace BattleGame.TournamentService.Apis
{
    public static class MatchApis
    {
        public static IEndpointRouteBuilder MapMatchApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/tournaments")
                .MapMatchApi()
                .WithTags("Match Api")
                .RequireRateLimiting("fixed");

            return builder;
        }

        public static RouteGroupBuilder MapMatchApi(this RouteGroupBuilder group)
        {
            group.MapPut("/{tournamentId:guid}/matches/{matchId:guid}/start", EndMatchesAsync)
                .WithName("Get match details");

            group.MapPut("/{tournamentId:guid}/matches/{matchId:guid}/end", EndMatchesAsync)
                .WithName("Get match details");

            return group;
        }

        private static async Task EndMatchesAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task GetMatchesAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
