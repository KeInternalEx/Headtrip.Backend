using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.State
{
    public static class UnrealServiceConstants
    {
        public static readonly float SERVER_LATENCY_WORST_QUALITY = 99999;
        public static readonly float SERVER_LATENCY_BAD_QUALITY = 100;
        public static readonly float SERVER_LATENCY_AVG_QUALITY = 80;
        public static readonly float SERVER_LATENCY_HIGH_QUALITY = 60;

        public static readonly float[] SERVER_LATENCY_TIERS =
        {
            SERVER_LATENCY_HIGH_QUALITY,
            SERVER_LATENCY_AVG_QUALITY,
            SERVER_LATENCY_BAD_QUALITY,
            SERVER_LATENCY_WORST_QUALITY
        };

    }
}
