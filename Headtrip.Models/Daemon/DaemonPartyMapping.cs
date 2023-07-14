using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonPartyMapping
    {
        public string? ZoneName { get; set; }
        public List<DaemonContract>? Contracts { get; set; }

    }
}
