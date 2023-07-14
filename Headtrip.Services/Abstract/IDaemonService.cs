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



        Task<GetDaemonContractGroupsResult> GetDaemonContractGroups();
    }
}
