using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Database;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Generic;
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
    public sealed class UeLatencyRecordRepository :
        ABulkSqlRepository<MUeLatencyRecord, HeadtripGameServerContext>, IUeLatencyRecordRepository
    {

        public UeLatencyRecordRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UeLatencyRecords", null) { }

        protected async override Task<IEnumerable<MUeLatencyRecord>> FinalizeBulkCopy()
            => await QueryAsync<MUeLatencyRecord>("UeLatencyRecord_FinalizeBulkCopy");

        public async Task<MUeLatencyRecord> Create(MUeLatencyRecord Object)
            => await QuerySingleAsync<MUeLatencyRecord, MUeLatencyRecord>("UeLatencyRecord_Create", Object);

        public async Task<MUeLatencyRecord> Update(MUeLatencyRecord Object)
            => await QuerySingleAsync<MUeLatencyRecord, MUeLatencyRecord>("UeLatencyRecord_Update", Object);

        public async Task<MUeLatencyRecord> Read(Guid ObjectId)
            => await QuerySingleAsync<MUeLatencyRecord, Guid>("UeLatencyRecord_Read", ObjectId);

        public async Task<IEnumerable<MUeLatencyRecord>> ReadAll()
            => await QueryAsync<MUeLatencyRecord>("UeLatencyRecord_ReadAll");

        public async Task<MUeLatencyRecord> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUeLatencyRecord, Guid>("UeLatencyRecord_Delete", ObjectId);

        public async Task<IEnumerable<MUeLatencyRecord>> ReadForTransformableRequests()
            => await QueryAsync<MUeLatencyRecord>("UeLatencyRecord_ReadForTransformableRequests");
    }
}
