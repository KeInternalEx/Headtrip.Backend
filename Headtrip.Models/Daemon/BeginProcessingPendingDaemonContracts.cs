using Headtrip.Models.Abstract;
using Headtrip.Models.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class BeginProcessingPendingDaemonContractsResult : AServiceCallResult
    {
        public List<DaemonContract>? Contracts { get; set; }
        public List<Channel>? Channels { get; set; }
    }
}
