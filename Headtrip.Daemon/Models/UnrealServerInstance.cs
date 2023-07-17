using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Models
{
    public class UnrealServerInstance
    {
        public Guid ChannelId { get; set; }
        public Process? Process { get; set; }
        public DateTime StartTime { get; set; }
    }
}
