using MediatR;

namespace BattleGame.TournamentService
{
    public class ApiServices
    {
        public readonly IMediator Mediator;
        public ApiServices(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
