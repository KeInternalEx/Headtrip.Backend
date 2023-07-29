using Headtrip.Objects;
using Headtrip.Objects.Abstract;
using Headtrip.Objects.UeService;
using Headtrip.Objects.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Services.Abstract
{
    public interface IUeServiceTaskService
    {

        /// <summary>
        /// Creates UeService ServerTransferRequest groups based on all of the current claims that have been laid at the time of being called.
        /// Does not create any changes in the database.
        /// </summary>
        /// <returns></returns>
        Task<RGetUeServiceServerTransferRequestGroupsResult> GetUeServiceServerTransferRequestGroups();

        /// <summary>
        /// Removes the claims with the associated UeService ServerTransferRequests.
        /// Updates the UeService ServerTransferRequests in the databases with the associated TargetUeServiceId on each ServerTransferRequest.
        /// Calls gsUeServiceProc_ProcessUeServiceServerTransferRequestGroup several times.
        /// </summary>
        /// <param name="UeServiceServerTransferRequestGroups"></param>
        /// <returns></returns>
        Task<ServiceCallResult> ProcessUeServiceServerTransferRequestGroups(List<UeServiceServerTransferRequestGroup> UeServiceServerTransferRequestGroups);



        Task<ServiceCallResult> ProcessPendingUeServiceServerTransferRequests();
    }
}
