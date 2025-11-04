using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.ReadRepositories
{
    public class TournamentParticipantReadRepository : PostgresRepository<TournamentParticipant>, ITournamentParticipantWriteRepository
    {
        public TournamentParticipantReadRepository(TournamentReadDbContext context) : base(context)
        {
        }
    }
}
