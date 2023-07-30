using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class MUeServiceLatencyRecord : ADatabaseObject
    {
        public Guid UeServiceId { get; set; }
        public Guid AccountId { get; set; }
        public float Latency { get; set; }


    }
}
