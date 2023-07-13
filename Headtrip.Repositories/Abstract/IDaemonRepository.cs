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
        Task CreateDaemon(Daemon daemon);
        Task<Daemon> GetDaemonByDaemonId(Guid daemonId);
        Task<Daemon> GetDaemonByNickname(string nickname);
        Task<Daemon> GetOrCreateDaemon(string nickname);

        Task<IEnumerable<DaemonContract>> BeginProcessingPendingContracts(Guid daemonId);
        Task<IEnumerable<DaemonContract>> BeginTransformingPendingContracts(Guid daemonId, int numberOfFreeInstances);



    }
}
