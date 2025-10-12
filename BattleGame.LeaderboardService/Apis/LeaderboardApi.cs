using BattleGame.LeaderboardService.Services;

namespace BattleGame.LeaderboardService.Apis
{
    public static class LeaderboardApi
    {
        public static IEndpointRouteBuilder MapLeaderboardApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/leaderboards")
                .MapLeaderboardApi()
                .WithTags("Leaderboards Api");
            return builder;
        }

        public static RouteGroupBuilder MapLeaderboardApi(this RouteGroupBuilder group)
        {
            group.MapGet("", GetAllLeaderboard)
                .WithName("Get all leaderboard");
            group.MapGet("/{gameId:guid}", GetAllLeaderboardByGameId)
                .WithName("Get all leaderboard by game id");

            return group;
        }

        private static async Task<IResult> GetAllLeaderboardByGameId(Guid gameId, ILeaderboardServices leaderboard)
        {
            var response = await leaderboard.GetTopXLeaderboardByGameId(gameId, 10);
            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response.Message);
            }
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> GetAllLeaderboard(ILeaderboardServices leaderboard)
        {
            var response = await leaderboard.GetAllLeaderboard();
            if (!response.IsSuccess)
            {
                TypedResults.BadRequest(response.Message);
            }
            return TypedResults.Ok(response);
        }
    }
}
