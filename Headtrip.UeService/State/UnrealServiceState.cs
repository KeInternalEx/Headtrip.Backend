using Headtrip.Objects.UnrealService;
using Headtrip.UnrealService.Threading;
using Headtrip.UnrealService.UnrealEngine;
using Headtrip.UnrealService.UnrealEngine.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.State
{
    public static class UnrealServiceState
    {

        public static TsObject<CancellationTokenSource> CancellationTokenSource =
            new TsObject<CancellationTokenSource>(new CancellationTokenSource());

        public static TsObject<MUnrealService> ServiceModel =
            new TsObject<MUnrealService>(null);

        public static Guid ServiceId { get; private set; }
    }
}
