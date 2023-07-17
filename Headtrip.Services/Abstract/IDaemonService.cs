using Headtrip.Models.Abstract;
using Headtrip.Models.Daemon;
using Headtrip.Models.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services.Abstract
{
    public interface IDaemonService
    {

        /// <summary>
        /// Creates daemon contract groups based on all of the current claims that have been laid at the time of being called.
        /// Does not create any changes in the database.
        /// </summary>
        /// <returns></returns>
        Task<GetDaemonContractGroupsResult> GetDaemonContractGroups();

        /// <summary>
        /// Removes the claims with the associated daemon contracts.
        /// Updates the daemon contracts in the databases with the associated TargetDaemonId on each contract.
        /// Calls gsDaemonProc_ProcessDaemonContractGroup several times.
        /// </summary>
        /// <param name="daemonContractGroups"></param>
        /// <returns></returns>
        Task<ServiceCallResult> ProcessDaemonContractGroups(List<DaemonContractGroup> daemonContractGroups);
    }
}
