using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonLatencyRecord : DatabaseObject
    {
        public Guid DaemonId { get; set; }
        public Guid AccountId { get; set; }
        public float Latency { get; set; }


    }
}
