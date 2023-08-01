using Headtrip.GameServerContext;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Generic;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.GameServer
{
    public sealed class UeStrRepository :
        ABulkSqlRepository<MUeServerTransferRequest, HeadtripGameServerContext>, IBulkCopy<MUeServerTransferRequest> IUeStrRepository
    {

        public UeStrRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.UeServerTransferRequests", null) { }


        #region IGenericRepository<MUeServerTransferRequest, Guid>

        public async Task<MUeServerTransferRequest> Create(MUeServerTransferRequest Object)
            => await QuerySingleAsync<MUeServerTransferRequest, MUeServerTransferRequest>("UeServerTransferRequest_Create", Object);

        public async Task<MUeServerTransferRequest> Read(Guid ObjectId)
            => await QuerySingleAsync<MUeServerTransferRequest, Guid>("UeServerTransferRequest_Read", ObjectId);

        public async Task<IEnumerable<MUeServerTransferRequest>> ReadAll()
            => await QueryAsync<MUeServerTransferRequest>("UeServerTransferRequest_ReadAll");

        public async Task<MUeServerTransferRequest> Update(MUeServerTransferRequest Object)
            => await QuerySingleAsync<MUeServerTransferRequest, MUeServerTransferRequest>("UeServerTransferRequest_Update", Object);

        public async Task<MUeServerTransferRequest> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUeServerTransferRequest, Guid>("UeServerTransferRequest_Delete", ObjectId);

        #endregion

        #region ABulkSqlRepository<MUeServerTransferRequest, HeadtripGameServerContext>

        protected async override Task<IEnumerable<MUeServerTransferRequest>> FinalizeBulkCopy()
            => await QueryAsync<MUeServerTransferRequest>("UeServerTransferRequest_FinalizeBulkCopy");

        #endregion

        #region IUeStrRepository

        public async Task<IEnumerable<MUeServerTransferRequest>> ReadWithState(EUeServerTransferRequestState State)
            => await QueryAsync<MUeServerTransferRequest, object>("UeServerTransferRequest_ReadWithState", new
            {
                State = (byte)State
            });

        #endregion

    }
}
