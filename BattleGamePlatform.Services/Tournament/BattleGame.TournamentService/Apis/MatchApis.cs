using BattleGame.TournamentService.CQRSServices.Match.Command;
using MediatR;

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
            group.MapPut("/{tournamentId:guid}/matches/{matchId:guid}/start", StartMatchesAsync)
                .WithName("Start match");

            return group;
        }

        private static async Task<IResult> StartMatchesAsync(Guid tournamentId, Guid matchId, IMediator mediator)
        {
            var command = new StartMatchCommand(tournamentId, matchId);
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }
    }
}
