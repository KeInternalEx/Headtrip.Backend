using Headtrip.Models.Abstract;
using Headtrip.Models.Daemon;
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
        /// Collects all currently laid claims for the daemon.
        /// Then based on those claims, takes ownership of the amount of contracts specified by each claim.
        /// Each claim is turned into a DaemonContractGroup object.
        /// Partyness is taken into account when selecting which contracts get put into their groups.
        /// </summary>
        /// <param name="daemonId">The unique identifier of the calling daemon</param>
        /// <param name="freeServerSlots">How many more servers the calling daemon can accomodate</param>
        /// <returns>ServiceCallResult wrapping a list of DaemonContractGroup objects</returns>
        Task<CreateDaemonClaimsResult> CreateDaemonClaims(Guid daemonId, int freeServerSlots);


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
