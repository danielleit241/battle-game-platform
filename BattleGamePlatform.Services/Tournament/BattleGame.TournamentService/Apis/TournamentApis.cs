using BattleGame.TournamentService.Dtos;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

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
            return group;
        }
        private static async Task<IResult> RegisterPlayerAsync([FromBody] RegisterTournamentDto dto, IMediator mediator)
        {
            throw new NotImplementedException();
        }

        private static async Task CreateTournamentAsync([FromBody] CreateTournamentDto dto, [AsParameters] ApiServices apiServices)
        {
            throw new NotImplementedException();
        }
    }
}
