using BattleGame.Shared.Common;
using BattleGame.TournamentHistoryService.Dtos;
using MediatR;

namespace BattleGame.TournamentHistoryService.CQRSServices.Tournament.Query
{
    public record GetOneTournamentQuery(Guid TournamentId) : IRequest<ApiResponse<TournamentSnapshotDto>>;
    public class GetOneTournamentQueryHandler : IRequestHandler<GetOneTournamentQuery, ApiResponse<TournamentSnapshotDto>>
    {
        public Task<ApiResponse<TournamentSnapshotDto>> Handle(GetOneTournamentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
