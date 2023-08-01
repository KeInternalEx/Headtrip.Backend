using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Repositories.Repositories.Interface;
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
    public class ZoneRepository : ASqlRepository<MZone, HeadtripGameServerContext>, IZoneRepository
    {
        public ZoneRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.Zones") { }




        #region IGenericRepository<MZone, string>

        public async Task<MZone> Create(MZone Object)
            => await QuerySingleAsync<MZone, MZone>("Zone_Create", Object);

        public async Task<MZone> Read(string ZoneName)
            => await QuerySingleAsync<MZone, string>("Zone_Read", ZoneName);

        public async Task<IEnumerable<MZone>> ReadAll()
            => await QueryAsync<MZone>("Zone_ReadAll");

        public async Task<MZone> Update(MZone Object)
            => await QuerySingleAsync<MZone, MZone>("Zone_Update", Object);

        public async Task<MZone> Delete(string ZoneName)
            => await QuerySingleAsync<MZone, string>("Zone_Delete", ZoneName);

        #endregion



        #region IZoneRepository
        #endregion

    }
}
