using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class TUeServicePartyMapping
    {
        public string? ZoneName { get; set; }
        public List<MUeServiceServerTransferRequest>? ServerTransferRequests { get; set; }
        public Dictionary<Guid, float>? AverageLatencyByUeServiceId { get; set; }
        public double AverageLevel { get; set; }
    }
}
