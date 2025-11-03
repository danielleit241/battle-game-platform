using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities.WriteEntities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentMatchRepository : PostgresRepository<TournamentMatch>, ITournamentMatchRepository
    {
        public TournamentMatchRepository(TournamentWriteDbContext context) : base(context)
        {
        }
    }
}
