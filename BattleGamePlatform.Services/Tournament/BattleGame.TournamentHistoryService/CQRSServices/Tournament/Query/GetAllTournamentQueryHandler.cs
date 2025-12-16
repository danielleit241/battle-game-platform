using BattleGame.Shared.Common;
using BattleGame.TournamentHistoryService.Dtos;
using MediatR;

namespace BattleGame.TournamentHistoryService.CQRSServices.Tournament.Query
{
    public record GetAllTournamentQuery() : IRequest<ApiResponse<TournamentSnapshotDto>>;
    public class GetAllTournamentQueryHandler : IRequestHandler<GetAllTournamentQuery, ApiResponse<TournamentSnapshotDto>>
    {
        public Task<ApiResponse<TournamentSnapshotDto>> Handle(GetAllTournamentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
