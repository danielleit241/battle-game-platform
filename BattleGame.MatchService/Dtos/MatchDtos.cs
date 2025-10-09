namespace BattleGame.MatchService.Dtos
{
    public record MatchDto(Guid Id, Guid UserId, string Username, Guid GameId, string GameName, int Score, DateTime Timestamp);
}
