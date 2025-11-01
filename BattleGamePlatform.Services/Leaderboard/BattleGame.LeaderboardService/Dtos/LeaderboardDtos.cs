namespace BattleGame.LeaderboardService.Dtos
{
    public record LeaderboardResponseDto(
         GameDto Game,
         ICollection<LeaderboardDto> LeaderboardDtos
    );

    public record GameDto(
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
