using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.GameSession;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.GameServer
{
    public class GameSessionRepository : ASqlRepository<MGameSession, HeadtripGameServerContext>, IGameSessionRepository
    {
        public GameSessionRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.GameSessions") { }


        #region GenericRepository<MGameSession, Guid>

        public async Task<MGameSession> Create(MGameSession Object)
            => await QuerySingleAsync<MGameSession, MGameSession>("GameSession_Create", Object);

        public async Task<MGameSession> Delete(Guid ObjectId)
            => await QuerySingleAsync<MGameSession, Guid>("GameSession_Delete", ObjectId);

        public async Task<MGameSession> Read(Guid ObjectId)
            => await QuerySingleAsync<MGameSession, Guid>("GameSession_Read", ObjectId);

        public async Task<IEnumerable<MGameSession>> ReadAll()
            => await QueryAsync<MGameSession>("GameSession_ReadAll");

        public async Task<MGameSession> Update(MGameSession Object)
            => await QuerySingleAsync<MGameSession, MGameSession>("GameSession_Update", Object);

        #endregion

    }
}
