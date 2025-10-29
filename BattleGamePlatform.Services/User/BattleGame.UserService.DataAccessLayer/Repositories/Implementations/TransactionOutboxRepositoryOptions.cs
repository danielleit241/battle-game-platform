namespace BattleGame.UserService.DataAccessLayer.Repositories.Implementations
{
    public class TransactionOutboxRepositoryOptions
    {
        public int MaxRetries { get; set; } = 3;
    }
}
