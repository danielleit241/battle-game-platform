
namespace BattleGame.TournamentService.Apis
{
    public static class TournamentApis
    {
        public static IEndpointRouteBuilder MapTournamentApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/tournaments")
                .MapTournamentApi()
                .WithTags("Tournament Api");
            return builder;
        }

        public static RouteGroupBuilder MapTournamentApi(this RouteGroupBuilder group)
        {
            group.MapPost("", CreateTournamentAsync)
                .WithName("Create tournament");

            group.MapPost("/{tournamentId:guid}/register", RegisterPlayerAsync)
                .WithName("Register player to tournament");

            group.MapPost("/{tournamentId:guid}/start", StartTournamentAsync)
                .WithName("Start tournament");

            group.MapPut("/{tournamentId:guid}/change-state", UpdateStatusAsync)
                .WithName("Update tournament status");

            return group;
        }

        private static async Task StartTournamentAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task RegisterPlayerAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateStatusAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task CreateTournamentAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
