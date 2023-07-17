using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonContractGroup
    {
        public string? ZoneName { get; set; }
        public Guid DaemonId { get; set; }
        public Guid DaemonContractGroupId { get; set; }

        public List<DaemonContract>? Contracts { get; set; }

        public byte NumberOfParties { get; set; }
        public byte NumberOfPlayers { get; set; }

    }
}
