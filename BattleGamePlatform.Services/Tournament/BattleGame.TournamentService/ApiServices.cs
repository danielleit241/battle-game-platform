using MediatR;

namespace BattleGame.TournamentService
{
    public class ApiServices
    {
        public IMediator Mediator { get; init; } = default!;
    }

}
