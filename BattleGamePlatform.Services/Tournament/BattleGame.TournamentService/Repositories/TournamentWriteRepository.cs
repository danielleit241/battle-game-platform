using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.Interfaces;

namespace BattleGame.TournamentService.Repositories
{
    public class TournamentWriteRepository : PostgresRepository<Tournament>, ITournamentWriteRepository
    {
        public TournamentWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }
    }
}
