using Headtrip.Objects.UeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IUeServiceRepository
    {
        Task<MUeService> GetOrCreateUeServiceByNickname(string nickname);
        Task<MUeService> GetUeServiceByUeServiceId(Guid UeServiceId);
        Task<IEnumerable<MUeService>> GetAllUeServices();

        Task<IEnumerable<MUeServiceServerTransferRequest>> BeginProcessingPendingServerTransferRequests(Guid UeServiceId);
        Task FinishProcessingPendingServerTransferRequests(Guid UeServiceId);


        Task<IEnumerable<MUeServiceServerTransferRequest>> GetAllTransformableUeServiceServerTransferRequests();
        Task<IEnumerable<MUeServiceServerTransferRequest>> GetAllTransformedUeServiceServerTransferRequests(Guid UeServiceId);


        Task ProcessUeServiceServerTransferRequestGroup(string UeServiceServerTransferRequestIds, Guid UeServiceId, Guid UeServiceServerTransferRequestGroupId, string zoneName);

        Task<IEnumerable<mUeServiceLatencyRecord>> GetLatencyRecordsForTransformableServerTransferRequests();

    }
}
