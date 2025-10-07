using BattleGame.GameService.BusinessLogicLayer.Services.Abstractions;
using BattleGame.GameService.Common;
using BattleGame.GameService.Common.Dtos;
using BattleGame.GameService.DataAccessLayer.Repositories.Abstractions;
using BattleGame.MessageBus.Abstractions;
using BattleGame.Shared.Common;

namespace BattleGame.GameService.BusinessLogicLayer.Services.Implementations
{
    public class GameServices(IGameRepository repository, IMessagePublisher publisher) : IGameServices
    {
        public async Task<ApiResponse<GameDto>> CompleteGame(Guid gameId, Guid userId)
        {
            var game = await repository.GetAsync(gameId);
            if (game is null)
            {
                return ApiResponse<GameDto>.FailureResponse("Game not found");
            }
            _ = publisher.Publish("game-completed", new { GameId = gameId, UserId = userId, CompletedAt = DateTime.UtcNow });

            var dto = game.AsDto();
            return ApiResponse<GameDto>.SuccessResponse(dto, "Game completed event published successfully");
        }

        public async Task<ApiResponse<GameDto>> CreateGame(CreateGameDto createGameDto)
        {
            var game = await repository.GetByNameAsync(createGameDto.Name);
            if (game is not null)
            {
                return ApiResponse<GameDto>.FailureResponse("Game with the same name already exists");
            }
            var newGame = createGameDto.AsEntity();
            await repository.AddAsync(newGame);

            var gameDto = newGame.AsDto();
            return ApiResponse<GameDto>.SuccessResponse(gameDto, "Game created successfully");
        }

        public async Task<ApiResponse<GameDto>> DeleteGame(Guid id)
        {
            var game = await repository.GetAsync(id);
            if (game is null)
            {
                return ApiResponse<GameDto>.FailureResponse("Game not found");
            }
            await repository.DeleteAsync(game);

            var dto = game.AsDto();
            return ApiResponse<GameDto>.SuccessResponse(dto, "Game deleted successfully");
        }

        public async Task<ApiResponse<IReadOnlyCollection<GameDto>>> GetAllGames()
        {
            var games = await repository.GetAllAsync();
            if (games is null || !games.Any())
            {
                return ApiResponse<IReadOnlyCollection<GameDto>>.FailureResponse("No games found");
            }
            var gameDtos = games.Select(g => g.AsDto()).ToList().AsReadOnly();
            return ApiResponse<IReadOnlyCollection<GameDto>>.SuccessResponse(gameDtos, "Games retrieved successfully");
        }

        public async Task<ApiResponse<GameDto>> GetGameById(Guid id)
        {
            var game = await repository.GetAsync(id);
            if (game is null)
            {
                return ApiResponse<GameDto>.FailureResponse("Game not found");
            }
            var dto = game.AsDto();
            return ApiResponse<GameDto>.SuccessResponse(dto, "Game retrieved successfully");
        }

        public async Task<ApiResponse<GameDto>> UpdateGame(Guid id, UpdateGameDto updateGameDto)
        {
            var game = await repository.GetAsync(id);
            if (game is null)
            {
                return ApiResponse<GameDto>.FailureResponse("Game not found");
            }

            MappingExtensions.UpdateEntity(game, updateGameDto);

            await repository.UpdateAsync(game);
            var dto = game.AsDto();
            return ApiResponse<GameDto>.SuccessResponse(dto, "Game updated successfully");
        }
    }
}
