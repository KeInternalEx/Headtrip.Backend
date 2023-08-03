using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.State
{
    public static class UeServiceConfiguration
    {

        public static int RequestTransformationTaskInterval
            => 1000;

        public static int RequestAssignmentTaskInterval
            => 1000;
        
        public static int ServerCreationTaskInterval
            => 1000;

        public static bool IsSuperService
            => true;
    }
}
