using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Objects.UnrealService;

namespace Headtrip.Objects.UnrealService.Transient
{
    public sealed class TPartyMapping
    {
        public string? ZoneName { get; set; }
        public List<MUnrealServerTransferRequest>? ServerTransferRequests { get; set; }
        public Dictionary<Guid, float>? AverageLatencyByUnrealServiceId { get; set; }
        public double AverageLevel { get; set; }
    }
}
