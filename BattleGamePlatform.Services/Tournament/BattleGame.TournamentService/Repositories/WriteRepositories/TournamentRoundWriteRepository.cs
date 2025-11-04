using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentRoundWriteRepository : PostgresRepository<TournamentRound>, ITournamentRoundWriteRepository
    {
        public TournamentRoundWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }

        public Task CreateRoundByNumberOfParticipants(Guid tournamentId, int maxParticipants)
        {
            throw new NotImplementedException();
        }
    }
}
