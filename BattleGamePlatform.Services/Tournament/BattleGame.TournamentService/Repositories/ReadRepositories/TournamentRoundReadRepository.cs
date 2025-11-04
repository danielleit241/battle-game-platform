using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.ReadRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentRoundReadRepository : PostgresRepository<TournamentRound>, ITournamentRoundReadRepository
    {
        public TournamentRoundReadRepository(TournamentReadDbContext context) : base(context)
        {
        }
    }
}
