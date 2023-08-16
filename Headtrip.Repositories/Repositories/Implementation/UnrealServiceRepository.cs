using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation
{
    public class UnrealServiceRepository : ASqlRepository<MUnrealService, HeadtripGameServerContext>, IUnrealServiceRepository
    {
        public UnrealServiceRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UnrealServices") { }


        #region IGenericRepository<MUnrealService, Guid>
        public async Task<MUnrealService> Create(MUnrealService Object)
            => await QuerySingleAsync<MUnrealService, MUnrealService>("UnrealService_Create", Object);

        public async Task<MUnrealService> Read(Guid ObjectId)
            => await QuerySingleAsync<MUnrealService, Guid>("UnrealService_Read", ObjectId);

        public async Task<IEnumerable<MUnrealService>> ReadAll()
            => await QueryAsync<MUnrealService>("UnrealService_ReadAll");

        public async Task<MUnrealService> Update(MUnrealService Object)
            => await QuerySingleAsync<MUnrealService, MUnrealService>("UnrealService_Update", Object);

        public async Task<MUnrealService> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUnrealService, Guid>("UnrealService_Delete", ObjectId);
        #endregion


        #region IUnrealServiceRepository
        public async Task<MUnrealService> ReadByNickname(string Nickname)
            => await QuerySingleAsync<MUnrealService, string>("UnrealService_ReadByNickname", Nickname);
        #endregion





    }
}
