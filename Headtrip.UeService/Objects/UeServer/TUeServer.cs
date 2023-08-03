using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.UeService.UnrealEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Objects.UeServer
{
    public sealed class TUeServer
    {
        public TStrGroup? InitialGroup { get; set; }
        public MChannel? Channel { get; set; }
        public UnrealServerInstance? Instance { get; set; }
    }
}
