using BattleGame.Shared.Database.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BattleGame.UserService.Common.Entities
{
    public class User : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
