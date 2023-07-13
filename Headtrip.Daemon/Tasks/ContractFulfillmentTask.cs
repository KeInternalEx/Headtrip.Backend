
using Headtrip.Daemon.Models;
using Headtrip.Daemon.Tasks.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Tasks
{
    public class ContractFulfillmentTask : IDaemonTask
    {



        public async Task<DaemonTaskResult> Execute()
        {
            // TODO: LOOK FOR CONTRACTS THAT HAVE A CHANNEL ID THAT I OWN
            // TODO: FULFILL THAT CONTRACT USING IPC


        }
    }
}
