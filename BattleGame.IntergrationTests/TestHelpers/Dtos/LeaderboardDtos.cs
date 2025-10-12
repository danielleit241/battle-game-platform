namespace BattleGame.IntergrationTests.TestHelpers.Dtos
{
    public record LeaderboardWithGameDto(
         LeaderboardGameDto Game,
         ICollection<LeaderboardDto> LeaderboardDtos
    );

    public record LeaderboardGameDto(
         Guid GameId,
         string GameName
    );

    public record LeaderboardDto(
        Guid Id,
        Guid UserId,
        string Username,
        int TotalScore,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

}
