using Headtrip.Objects.GameSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IGameSessionRepository
    {
        Task<GameSession> GetOrCreateGameSession(Guid accountId);


    }
}
