namespace BattleGame.GameService.Search
{
    public sealed class GameIndexDocument
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; } = "";
        public int? MaxPlayers { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public SimpleCompletion Suggest { get; set; } = new();
    }

    public sealed class SimpleCompletion
    {
        public string[] Input { get; set; } = [];
    }
}
