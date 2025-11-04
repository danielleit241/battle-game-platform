using BattleGame.Shared.Database.Repositories;
using BattleGame.TournamentService.Entities;
using BattleGame.TournamentService.Infrastructure.Data;
using BattleGame.TournamentService.Repositories.WriteRepositories.Interfaces;

namespace BattleGame.TournamentService.Repositories.WriteRepositories
{
    public class TournamentParticipantWriteRepository : PostgresRepository<TournamentParticipant>, ITournamentParticipantWriteRepository
    {
        public TournamentParticipantWriteRepository(TournamentWriteDbContext context) : base(context)
        {
        }
    }
}