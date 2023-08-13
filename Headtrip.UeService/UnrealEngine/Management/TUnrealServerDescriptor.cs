using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Management
{
    public enum EUnrealServerState : byte
    {
        PendingStartup = 0,
        Started = 1,
        Broken = 2,
        ShuttingDown = 3
    }


    public sealed class TUnrealServerDescriptor
    {
        public Guid ServerId { get; set; }
        public UnrealServerInstance? Instance { get; set; }
        public TStrGroup? ProgenitorGroup { get; set; }
        public MChannel? Channel { get; set; }
        public EUnrealServerState ServerState { get; set; }
    }
}
