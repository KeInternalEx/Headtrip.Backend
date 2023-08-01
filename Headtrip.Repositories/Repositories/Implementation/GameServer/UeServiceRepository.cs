using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.UeService;
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
    public class UeServiceRepository : ASqlRepository<MUeService, HeadtripGameServerContext>, IUeServiceRepository
    {
        public UeServiceRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UeServices") { }


        #region IGenericRepository<MUeService, Guid>
        public async Task<MUeService> Create(MUeService Object)
            => await QuerySingleAsync<MUeService, MUeService>("UeService_Create", Object);

        public async Task<MUeService> Read(Guid ObjectId)
            => await QuerySingleAsync<MUeService, Guid>("UeService_Read", ObjectId);

        public async Task<IEnumerable<MUeService>> ReadAll()
            => await QueryAsync<MUeService>("UeService_ReadAll");

        public async Task<MUeService> Update(MUeService Object)
            => await QuerySingleAsync<MUeService, MUeService>("UeService_Update", Object);

        public async Task<MUeService> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUeService, Guid>("UeService_Delete", ObjectId);
        #endregion


        #region IUeServiceRepository
        public async Task<MUeService> ReadByNickname(string Nickname)
            => await QuerySingleAsync<MUeService, string>("UeService_ReadByNickname", Nickname);
        #endregion





    }
}
