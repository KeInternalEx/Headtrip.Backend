
using Headtrip.Daemon.Models;
using Headtrip.Daemon.Tasks.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Tasks
{
    public class ContractTansformationTask : IDaemonTask
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IDaemonService _daemonService;


        public ContractTansformationTask(
            ILogging<HeadtripGameServerContext> logging, 
            IDaemonService daemonService)
        {
            _logging = logging;
            _daemonService = daemonService;
        }



        public async Task<DaemonTaskResult> Execute()
        {
            // var claims = await 


            


            // TODO: THEN THE SERVICE WILL PICK UP THE CONTRACTS IT HAS A "TRANSFORMING ID" MATCHING
            // TODO: IT WILL COLLECT THE CHANNELS BASED ON THOSE CONTRACTS AND IT WILL CREATE AN UNREAL SERVER FOR EACH CHANNEL
            // TODO: IT WILL UPDATE THE CHANNEL RECORDS WITH THE CONNECTION STRINGS FOR THE UNREAL SERVERS
            // TODO: IT WILL THEN UPDATE THE CONTRACTS WITH THE DaemonId, ALLOWING IT TO BE PICKED UP BY THE CORRECT DAEMONS
             

        }
    }
}
