namespace BattleGame.UserService.Infrastructure.Entities
{
    public class Role : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<User> Users { get; set; } = [];
    }
}
