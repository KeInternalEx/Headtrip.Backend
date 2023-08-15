using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Database;
using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Generic;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.GameServer
{
    public sealed class UnrealLatencyRecordRepository :
        ABulkSqlRepository<MUnrealLatencyRecord, HeadtripGameServerContext>, IUnrealLatencyRecordRepository
    {

        public UnrealLatencyRecordRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UeLatencyRecords", null) { }

        public async Task<MUnrealLatencyRecord> Create(MUnrealLatencyRecord Object)
            => await QuerySingleAsync<MUnrealLatencyRecord, MUnrealLatencyRecord>("UeLatencyRecord_Create", Object);

        public async Task<MUnrealLatencyRecord> Update(MUnrealLatencyRecord Object)
            => await QuerySingleAsync<MUnrealLatencyRecord, MUnrealLatencyRecord>("UeLatencyRecord_Update", Object);

        public async Task<MUnrealLatencyRecord> Read(Guid ObjectId)
            => await QuerySingleAsync<MUnrealLatencyRecord, Guid>("UeLatencyRecord_Read", ObjectId);

        public async Task<IEnumerable<MUnrealLatencyRecord>> ReadAll()
            => await QueryAsync<MUnrealLatencyRecord>("UeLatencyRecord_ReadAll");

        public async Task<MUnrealLatencyRecord> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUnrealLatencyRecord, Guid>("UeLatencyRecord_Delete", ObjectId);

        public async Task<IEnumerable<MUnrealLatencyRecord>> ReadForTransformableRequests()
            => await QueryAsync<MUnrealLatencyRecord>("UeLatencyRecord_ReadForTransformableRequests");


        protected async override Task<IEnumerable<MUnrealLatencyRecord>> FinalizeBulkCopy()
            => await QueryAsync<MUnrealLatencyRecord>("UeLatencyRecord_FinalizeBulkCopy");

        protected async override Task<IEnumerable<MUnrealLatencyRecord>> FinalizeBulkUpdate()
            => await QueryAsync<MUnrealLatencyRecord>("UeLatencyRecord_FinalizeBulkUpdate");
    }
}
