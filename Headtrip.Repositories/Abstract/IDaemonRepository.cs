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
        Task<IEnumerable<Daemon>> GetAllDaemons();

        Task<IEnumerable<DaemonContract>> BeginProcessingPendingContracts(Guid daemonId);
        Task FinishProcessingPendingContracts(Guid daemonId);


        Task<IEnumerable<DaemonContract>> GetAllTransformableDaemonContracts();
        Task<IEnumerable<DaemonContract>> GetAllTransformedDaemonContracts(Guid daemonId);


        Task ProcessDaemonContractGroup(string daemonContractIds, Guid daemonId, Guid daemonContractGroupId, string zoneName);

        Task<IEnumerable<DaemonLatencyRecord>> GetLatencyRecordsForTransformableContracts();

    }
}
