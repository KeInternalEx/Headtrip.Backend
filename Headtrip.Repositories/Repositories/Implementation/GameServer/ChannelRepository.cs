using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.GameServer
{

    public class ChannelRepository : ASqlRepository<MChannel, HeadtripGameServerContext>, IChannelRepository
    {

        public ChannelRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.Channels") { }


        #region IGenericRepository<MChannel, Guid>

        public async Task<MChannel> Create(MChannel Object)
            => await QuerySingleAsync<MChannel, MChannel>("Channel_Create", Object);

        public async Task<MChannel> Read(Guid ObjectId)
            => await QuerySingleAsync<MChannel, Guid>("Channel_Read", ObjectId);

        public async Task<IEnumerable<MChannel>> ReadAll()
            => await QueryAsync<MChannel>("Channel_ReadAll");

        public async Task<MChannel> Update(MChannel Object)
            => await QuerySingleAsync<MChannel, MChannel>("Channel_Update", Object);

        public async Task<MChannel> Delete(Guid ObjectId)
            => await QuerySingleAsync<MChannel, Guid>("Channel_Delete", ObjectId);

        #endregion

    }
}
