using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{

    public class ChannelRepository : IChannelRepository
    {
        private readonly IContext<HeadtripGameServerContext> _context;

        public ChannelRepository(IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Channel>> GetAllChannels()
        {
            return await _context.Connection.QueryAsync<Channel>(
                sql: "[gsChannels_GetAllChannels]",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
