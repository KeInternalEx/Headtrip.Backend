using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Models
{
    public class DaemonTaskResult
    {
        public bool Success { get; set; }
        public TimeSpan TimeSpent { get; set; }
    }
}
