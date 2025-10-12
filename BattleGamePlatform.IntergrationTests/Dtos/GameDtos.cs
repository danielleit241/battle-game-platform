namespace BattleGame.MatchService.Dtos
{
    public record GameDto(Guid Id, string Name, string Description, int MaxPlayers);
}
