using Headtrip.Objects.UeService;
using Headtrip.UeService.Objects.UeServer;
using Headtrip.UeService.Threading;
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

        #region Unreal Server Instances
        
        // THE DICTS IN THIS REGION SHOULD BE THE ONLY LONG TERM REFERENCES TO UnrealServerInstance OBJECTS
        // IF THE INSTANCE IS REMOVED FROM THESE DICTS, IT SHOULD BE GARBAGE COLLECTED AND AUTOMATICALLY SHUTDOWN GRACEFULLY
        
        public static ConcurrentDictionary<Guid, TUeServer> ActiveServersByStrGroupId =
            new ConcurrentDictionary<Guid, TUeServer>();

        public static ConcurrentDictionary<Guid, TUeServer> ActiveServersByChannelId =
            new ConcurrentDictionary<Guid, TUeServer>();
        
        #endregion


        public static TsObject<CancellationTokenSource> CancellationTokenSource =
            new TsObject<CancellationTokenSource>(new CancellationTokenSource());

        public static TsObject<MUeService> ServiceModel =
            new TsObject<MUeService>(null);

        public static Guid ServiceId { get; private set; }
    }
}
