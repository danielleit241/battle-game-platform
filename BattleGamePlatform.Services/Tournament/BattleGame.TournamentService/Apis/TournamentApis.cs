using BattleGame.TournamentService.CQRSServices.Tournament.Command;
using BattleGame.TournamentService.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BattleGame.TournamentService.Apis
{
    public static class TournamentApis
    {
        public static IEndpointRouteBuilder MapTournamentApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/tournaments")
                .MapTournamentApi()
                .WithTags("Tournament Api")
                .RequireRateLimiting("fixed");

            return builder;
        }

        public static RouteGroupBuilder MapTournamentApi(this RouteGroupBuilder group)
        {
            group.MapPost("", CreateTournamentAsync)
                .WithName("Create tournament");

            group.MapPost("/{tournamentId:guid}/register", RegisterPlayerAsync)
                .WithName("Register player to tournament");
            return group;
        }
        private static async Task<IResult> RegisterPlayerAsync(Guid tournamentId, [FromBody] RegisterTournamentDto dto, IMediator mediator)
        {
            var command = new RegisterTournamentCommand(dto, tournamentId);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        }

        private static async Task<IResult> CreateTournamentAsync([FromBody] CreateTournamentDto dto, [AsParameters] ApiServices apiServices)
        {
            var command = new CreateTournamentCommand(dto);
            var response = await apiServices.Mediator.Send(command);
            return Results.Ok(response);
        }
    }
}
