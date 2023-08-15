using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Instance
{
    public sealed class MChannel : ADatabaseObject
    {
        public Guid ChannelId { get; set; }
        public Guid UnrealServiceId { get; set; }
        public Guid UnrealServerId { get; set; }
        public string? ZoneName { get; set; }
        public string? ConnectionString { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool IsAvailable { get; set; } // Set to false while spin up is in process, set to true once pending ServerTransferRequests have been executed
    }
}
