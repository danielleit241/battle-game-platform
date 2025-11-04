using BattleGame.TournamentService.Dtos;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Match.Command
{
    public record StartMatchCommand(StartMatchDto Dto) : IRequest<bool>;
    public class StartMatchHandler : IRequestHandler<StartMatchCommand, bool>
    {
        public Task<bool> Handle(StartMatchCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    {
    }
}
