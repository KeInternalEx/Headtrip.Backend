using Headtrip.Models.Daemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IDaemonRepository
    {
        Task<Daemon> GetOrCreateDaemonByNickname(string nickname);
        Task<Daemon> GetDaemonByDaemonId(Guid daemonId);


        Task<IEnumerable<DaemonContract>> BeginProcessingPendingContracts(Guid daemonId);
        Task<IEnumerable<DaemonContract>> GetAllTransformableDaemonContracts();

        Task<IEnumerable<DaemonClaim>> CreateClaimsForTransformableContracts(Guid daemonId, int numberOfFreeInstances);
        Task<IEnumerable<DaemonClaim>> GetAllDaemonClaims();

    }
}
