using MassTransit.Mediator;

namespace BattleGame.TournamentService
{
    public class ApiServices
    {
        private readonly IMediator mediator;
        public ApiServices(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
