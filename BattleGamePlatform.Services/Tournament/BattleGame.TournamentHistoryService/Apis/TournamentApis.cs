namespace BattleGame.TournamentHistoryService.Apis
{
    public static class TournamentApis
    {
        public static IEndpointRouteBuilder MapTournamentApis(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/tournaments")
                .MapTournamentApis()
                .WithTags("Tournament History Api")
                .RequireRateLimiting("fixed");
            return builder;
        }

        public static RouteGroupBuilder MapTournamentApis(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllTournamentsAsync)
                .WithName("Get all tournaments");
            group.MapGet("/{tournamentId:guid}", GetOneTournamentAsync)
                .WithName("Get one tournament");

            return group;
        }

        public static async Task<IResult> GetAllTournamentsAsync()
        {
            throw new NotImplementedException();
        }

        public static async Task<IResult> GetOneTournamentAsync(Guid tournamentId)
        {
            throw new NotImplementedException();
        }
    }
}
