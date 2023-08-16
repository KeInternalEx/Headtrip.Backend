using Headtrip.Objects.GameSession;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface
{
    public interface IGameSessionRepository : IGenericRepository<MGameSession, Guid>
    {

    }
}
