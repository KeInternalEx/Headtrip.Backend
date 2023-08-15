using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Objects.UnrealService;

namespace Headtrip.Objects.UnrealService.Transient
{
    public sealed class TStrGroup
    {
        public string? ZoneName { get; set; }
        public Guid UnrealServiceId { get; set; }
        public Guid GroupId { get; set; }
        public List<MUnrealServerTransferRequest>? ServerTransferRequests { get; set; }

        public byte NumberOfParties { get; set; }
        public byte NumberOfPlayers { get; set; }

    }
}
