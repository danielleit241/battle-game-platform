using BattleGame.GameService.Search;
using BattleGame.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace BattleGame.GameService.SearchService.Apis
{
    public static class SearchGameApi
    {
        private const int defaultPageSize = 10;

        public static IEndpointRouteBuilder MapGameApi(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/api/v1/games")
                .MapGameApi()
                .WithTags("Game Api");
            return builder;
        }

        public static RouteGroupBuilder MapGameApi(this RouteGroupBuilder group)
        {
            group.MapGet("/search", FullTextSearchGames)
                .WithName("Search games by name");

            return group;
        }

        private static async Task<IResult> FullTextSearchGames(
            [AsParameters] ApiServices apiServices,
            [FromQuery] string? query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = defaultPageSize)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Results.BadRequest("Query parameter is required");
            }

            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? defaultPageSize : pageSize;
            var from = (page - 1) * pageSize;

            var search = await apiServices.ElasticsearchClient.SearchAsync<GameIndexDocument>(s => s
                .Query(q => q.QueryString(qs => qs.Query(query)))
                .From(from)
                .Size(pageSize),
                apiServices.CancellationToken);

            if (!search.IsValidResponse)
            {
                var errorResponse = ApiResponse<IEnumerable<GameIndexDocument>>.FailureResponse(
                    message: "Search failed",
                    metadata: new
                    {
                        Errors = search.ElasticsearchServerError?.Error.Reason
                    });
                return Results.BadRequest(errorResponse);
            }
            var results = search.Documents.ToList();

            if (!results.Any())
            {
                var errorResponse = ApiResponse<IEnumerable<GameIndexDocument>>.FailureResponse(
                    message: "Search failed",
                    metadata: new
                    {
                        Errors = search.ElasticsearchServerError?.Error.Reason
                    });
                return Results.BadRequest(errorResponse);
            }

            var response = ApiResponse<IEnumerable<GameIndexDocument>>.SuccessResponse(
                data: results,
                message: "Search successful",
                metadata: new
                {
                    Total = search.Total,
                    Page = page,
                    PageSize = pageSize
                });

            return Results.Ok(response);
        }
    }

}
