using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface
{
    public interface IUnrealLatencyRecordRepository : IGenericRepository<MUnrealLatencyRecord, Guid>, IBulkOperations<MUnrealLatencyRecord>
    {
        Task<IEnumerable<MUnrealLatencyRecord>> ReadForTransformableRequests();

    }
}
