namespace BattleGame.UserService.BusinessLogicLayer.Services.Abstractions
{
    public interface ITokenServices
    {
        string GenerateAccessToken(User user);
    }
}
