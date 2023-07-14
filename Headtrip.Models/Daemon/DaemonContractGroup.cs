using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonContractGroup
    {
        public DaemonClaim? Claim { get; set; }
        public List<DaemonContract>? Contracts { get; set; }

        public int NumberOfParties { get; set; }
        public int NumberOfPlayers { get; set; }

    }
}
