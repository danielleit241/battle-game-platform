namespace BattleGame.TournamentService.Dtos
{
    public record StartMatchDto(Guid MatchId);
    public record EndMatchDto(Guid MatchId, Guid WinnerId);
}
