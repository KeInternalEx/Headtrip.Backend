using Headtrip.GameServerContext;
using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Generic;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation
{
    public sealed class UnrealStrRepository :
        ABulkSqlRepository<MUnrealServerTransferRequest, HeadtripGameServerContext>, IBulkOperations<MUnrealServerTransferRequest>, IUnrealStrRepository
    {

        public UnrealStrRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UeServerTransferRequests", null) { }


        #region IGenericRepository<MUeServerTransferRequest, Guid>

        public async Task<MUnrealServerTransferRequest> Create(MUnrealServerTransferRequest Object)
            => await QuerySingleAsync<MUnrealServerTransferRequest, MUnrealServerTransferRequest>("UeServerTransferRequest_Create", Object);

        public async Task<MUnrealServerTransferRequest> Read(Guid ObjectId)
            => await QuerySingleAsync<MUnrealServerTransferRequest, Guid>("UeServerTransferRequest_Read", ObjectId);

        public async Task<IEnumerable<MUnrealServerTransferRequest>> ReadAll()
            => await QueryAsync<MUnrealServerTransferRequest>("UeServerTransferRequest_ReadAll");

        public async Task<MUnrealServerTransferRequest> Update(MUnrealServerTransferRequest Object)
            => await QuerySingleAsync<MUnrealServerTransferRequest, MUnrealServerTransferRequest>("UeServerTransferRequest_Update", Object);

        public async Task<MUnrealServerTransferRequest> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUnrealServerTransferRequest, Guid>("UeServerTransferRequest_Delete", ObjectId);

        #endregion

        #region ABulkSqlRepository<MUeServerTransferRequest, HeadtripGameServerContext>

        protected async override Task<IEnumerable<MUnrealServerTransferRequest>> FinalizeBulkCopy()
            => await QueryAsync<MUnrealServerTransferRequest>("UeServerTransferRequest_FinalizeBulkCopy");

        protected async override Task<IEnumerable<MUnrealServerTransferRequest>> FinalizeBulkUpdate()
            => await QueryAsync<MUnrealServerTransferRequest>("UeServerTransferRequest_FinalizeBulkUpdate");

        #endregion

        #region IUeStrRepository

        public async Task<IEnumerable<MUnrealServerTransferRequest>> ReadWithState(EUeServerTransferRequestState State)
            => await QueryAsync<MUnrealServerTransferRequest, object>("UeServerTransferRequest_ReadWithState", new
            {
                State = (byte)State
            });

        #endregion

    }
}
