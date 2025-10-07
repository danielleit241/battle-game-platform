namespace BattleGame.GameService.BusinessLogicLayer.Services.Abstractions
{
    public interface IGameServices
    {
        Task<ApiResponse<IReadOnlyCollection<GameDto>>> GetAllGames();
        Task<ApiResponse<GameDto>> GetGameById(Guid id);
        Task<ApiResponse<GameDto>> CreateGame(CreateGameDto createGameDto);
        Task<ApiResponse<GameDto>> UpdateGame(Guid id, UpdateGameDto updateGameDto);
        Task<ApiResponse<GameDto>> DeleteGame(Guid id);
        Task<ApiResponse<GameDto>> CompleteGame(Guid gameId, Guid userId);
    }
}
