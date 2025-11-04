using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentMatchReadRepository : PostgresRepository<TournamentMatch>, ITournamentMatchReadRepository
    {
        public TournamentMatchReadRepository(TournamentReadDbContext context) : base(context)
        {
        }
    }
}
