using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface.GameServer
{
    public interface IUnrealStrRepository : IGenericRepository<MUnrealServerTransferRequest, Guid>, IBulkOperations<MUnrealServerTransferRequest>
    {


        Task<IEnumerable<MUnrealServerTransferRequest>> ReadWithState(EUeServerTransferRequestState State);
    }
}
