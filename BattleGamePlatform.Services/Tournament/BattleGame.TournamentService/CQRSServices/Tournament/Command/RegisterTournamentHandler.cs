using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Tournament.Command
{
    public record RegisterTournamentCommand(RegisterTournamentDto Dto) : IRequest<bool>
    public class RegisterTournamentHandler : IRequestHandler<RegisterTournamentCommand, bool>
    {
        public Task<bool> Handle(RegisterTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
