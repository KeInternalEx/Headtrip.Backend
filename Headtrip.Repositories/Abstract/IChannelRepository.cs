using Headtrip.GameServerContext;
using Headtrip.Models.Instance;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IChannelRepository
    {
        Task<IEnumerable<Channel>> GetAllChannels();

    }
}
