namespace BattleGame.IntergrationTests.TestHelpers.Dtos
{
    public record GameDto(Guid Id, string Name, string Description, int MaxPlayers);
    public record CreateGameDto(string Name, string Description, int MaxPlayers);
    public record UpdateGameDto(string Name, string Description, int MaxPlayers);
    public record DeleteGameDto(Guid Id);
    public record CompleteGameDto(Guid Id);
}
