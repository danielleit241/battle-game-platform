using BattleGame.TournamentService.Dtos;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Match.Command
{
    public record EndMatchCommand(EndMatchDto Dto) : IRequest<bool>;
    public class EndMatchHandler : IRequestHandler<EndMatchCommand, bool>
    {
        public Task<bool> Handle(EndMatchCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    {
    }
}
