namespace BattleGame.GameService.Common
{
    public static class MappingExtensions
    {
        public static GameDto AsDto(this Entities.Game game) =>
            new GameDto(game.Id, game.Name, game.Description, game.MaxPlayers);

        public static Entities.Game AsEntity(this CreateGameDto createGameDto) =>
            new Entities.Game
            {
                Id = Guid.NewGuid(),
                Name = createGameDto.Name,
                Description = createGameDto.Description,
                MaxPlayers = createGameDto.MaxPlayers,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

        public static void UpdateEntity(this Entities.Game game, UpdateGameDto updateGameDto)
        {
            game.Name = updateGameDto.Name;
            game.Description = updateGameDto.Description;
            game.MaxPlayers = updateGameDto.MaxPlayers;
            game.UpdatedAt = DateTime.UtcNow;
        }
    }
}
