using Headtrip.UnrealService.UnrealEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.UnrealEngine.Management
{
    public enum EUnrealMessageBusState : byte
    {
        PendingStartup = 0,
        Started = 1,
        Broken = 2,
        ShuttingDown = 3
    }


    public sealed class TUnrealMessageBusDescriptor
    {
        public IUnrealServerInstance? ServerInstance { get; set; }
        public UnrealMessageBus? MessageBus { get; set; }
        public EUnrealMessageBusState State { get; set; }
    }
}
