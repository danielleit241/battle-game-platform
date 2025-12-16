using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.Interfaces;

namespace BattleGame.TournamentService.Repositories
{
    public class TournamentRepository : PostgresRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(TournamentWriteDbContext context) : base(context)
        {
        }
    }
}
