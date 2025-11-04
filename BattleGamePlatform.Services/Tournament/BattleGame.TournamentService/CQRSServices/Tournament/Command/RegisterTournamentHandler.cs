using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Tournament.Command
{
    public record RegisterTournamentCommand(RegisterTournamentDto Dto) : IRequest<ApiResponse<TournamentDto>>;
    public class RegisterTournamentHandler : IRequestHandler<RegisterTournamentCommand, ApiResponse<TournamentDto>>
    {
        public async Task<ApiResponse<TournamentDto>> Handle(RegisterTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
