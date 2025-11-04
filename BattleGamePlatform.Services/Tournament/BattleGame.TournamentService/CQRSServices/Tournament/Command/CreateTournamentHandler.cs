using BattleGame.Shared.Common;
using BattleGame.TournamentService.Dtos;
using MediatR;

namespace BattleGame.TournamentService.CQRSServices.Tournament.Command
{
    public record CreateTournamentCommand(CreateTournamentDto Dto) : IRequest<ApiResponse<TournamentDto>>;

    public class CreateTournamentHandler : IRequestHandler<CreateTournamentCommand, ApiResponse<TournamentDto>>
    {
        public Task<ApiResponse<TournamentDto>> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
