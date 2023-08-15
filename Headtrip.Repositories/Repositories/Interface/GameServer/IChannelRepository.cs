using Headtrip.GameServerContext;
using Headtrip.Objects.Account;
using Headtrip.Objects.Instance;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface.GameServer
{
    public interface IChannelRepository : IGenericRepository<MChannel, Guid>
    {
        Task<MChannel> IncrementPlayerCount(Guid ChannelId);
        Task<MChannel> DecrementPlayerCount(Guid ChannelId);

    }
}
