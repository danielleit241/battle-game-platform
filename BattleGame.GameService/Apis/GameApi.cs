namespace BattleGame.GameService.Apis
{
    public static class GameApi
    {
        public static IEndpointRouteBuilder MapGameApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/games")
                .MapGameApi()
                .WithTags("Game Api");
            return builder;
        }

        public static RouteGroupBuilder MapGameApi(this RouteGroupBuilder group)
        {
            group.MapGet("", GetAllGames)
                .WithName("Get all games");
            group.MapGet("/{id:guid}", GetGameById)
                .WithName("Get game by id");
            group.MapPost("", CreateGame)
                .WithName("Create game");
            group.MapPut("/{id:guid}", UpdateGame)
                .WithName("Update game");
            group.MapDelete("/{id:guid}", DeleteGame)
                .WithName("Delete game");
            group.MapPost("/{id:guid}/completed", CompletedGame)
                .WithName("Completed game");
            return group;
        }

        private static async Task<IResult> CompletedGame(Guid id, IGameServices services, GetClaims getClaims)
        {
            var userId = getClaims.GetUserId();
            if (userId == Guid.Empty)
            {
                return Results.Unauthorized();
            }
            var result = await services.CompleteGame(id, userId);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }

        private static async Task<IResult> DeleteGame(Guid id, IGameServices services)
        {
            var result = await services.DeleteGame(id);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }

        private static async Task<IResult> UpdateGame(Guid id, UpdateGameDto dto, IGameServices services)
        {
            var result = await services.UpdateGame(id, dto);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }

        private static async Task<IResult> CreateGame(CreateGameDto dto, IGameServices services)
        {
            var result = await services.CreateGame(dto);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result);
            }
            return Results.Ok(result);
        }

        private static async Task<IResult> GetGameById(Guid id, IGameServices services)
        {
            var result = await services.GetGameById(id);
            if (!result.IsSuccess)
            {
                return Results.NotFound(result);
            }
            return Results.Ok(result);
        }

        private static async Task<IResult> GetAllGames(IGameServices services)
        {
            var results = await services.GetAllGames();
            if (!results.IsSuccess)
            {
                return Results.NotFound(results);
            }
            return Results.Ok(results);
        }
    }
}
