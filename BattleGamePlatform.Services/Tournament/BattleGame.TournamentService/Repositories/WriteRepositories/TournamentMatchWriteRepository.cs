using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentMatchWriteRepository : PostgresRepository<TournamentMatch>, ITournamentMatchWriteRepository
    {
        public TournamentMatchWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }

        public Task AddMatchesByRoundId(Guid roundId, Guid player1, Guid player2)
        {
            throw new NotImplementedException();
        }
    }
}
