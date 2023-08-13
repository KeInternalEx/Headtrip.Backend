using Headtrip.Objects.UeService;
using Headtrip.UeService.Objects.UeServer;
using Headtrip.UeService.Threading;
using Headtrip.UeService.UnrealEngine;
using Headtrip.UeService.UnrealEngine.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.State
{
    public static class UeServiceState
    {

        public static TsObject<CancellationTokenSource> CancellationTokenSource =
            new TsObject<CancellationTokenSource>(new CancellationTokenSource());

        public static TsObject<MUeService> ServiceModel =
            new TsObject<MUeService>(null);

        public static Guid ServiceId { get; private set; }
    }
}
