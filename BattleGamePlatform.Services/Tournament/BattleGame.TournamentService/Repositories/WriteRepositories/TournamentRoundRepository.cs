using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities.WriteEntities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentRoundRepository : PostgresRepository<TournamentRound>, ITournamentRoundRepository
    {
        public TournamentRoundRepository(TournamentWriteDbContext context) : base(context)
        {
        }
    }
}
