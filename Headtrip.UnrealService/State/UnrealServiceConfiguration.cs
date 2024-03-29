﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.State
{
    public static class UnrealServiceConfiguration
    {

        public static int RequestTransformationTaskInterval
            => 1000;

        public static int RequestAssignmentTaskInterval
            => 1000;
        
        public static int ServerCreationTaskInterval
            => 1000;

        public static int ServerShutdownGracePeriod
            => 15000;

        public static bool IsSuperService
            => true;

        public static string ServerBinaryPath
            => "";
    }
}
