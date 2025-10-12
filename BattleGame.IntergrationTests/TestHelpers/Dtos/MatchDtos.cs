namespace BattleGame.IntergrationTests.TestHelpers.Dtos
{
    public record MatchDto(Guid Id, Guid UserId, string Username, IReadOnlyCollection<MatchGameDto> MatchGameDtos);

    public record MatchGameDto(Guid GameId, string GameName, int Score, DateTime Timestamp);

}
