namespace BattleGame.TournamentService.Dtos
{
    public class MatchCompleteDto(
            Guid MatchId,
            Guid TournamentId,
            Guid WinnerId,
            DateTime CompletedAt
        );
}
