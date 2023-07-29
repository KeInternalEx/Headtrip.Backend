using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.GameSession;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class GameSessionRepository : IGameSessionRepository
    {
        private readonly IContext<HeadtripGameServerContext> _context;

        public GameSessionRepository(IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }

        public async Task<GameSession> GetOrCreateGameSession(Guid accountId)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<GameSession>(
                sql: "[GameSession_GetOrCreateGameSession]",
                param: new
                {
                    AccountId = accountId
                },
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
