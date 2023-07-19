
using Headtrip.Daemon.Models;
using Headtrip.Daemon.Services.Abstract;
using Headtrip.Daemon.State.Abstract;
using Headtrip.Daemon.Tasks.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Tasks
{
    public class ContractFulfillmentTask : IDaemonTask
    {

        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IDaemonState _daemonState;
        private readonly IDaemonTaskService _daemonService;


        public ContractFulfillmentTask(
            ILogging<HeadtripGameServerContext> logging,
            IDaemonState daemonState,
            IDaemonTaskService daemonService)
        {
            _logging = logging;
            _daemonState = daemonState;
            _daemonService = daemonService;
        }


        public async Task<DaemonTaskResult> Execute()
        {
            var result = new DaemonTaskResult
            {
                IsSuccessful = false,
                Status = string.Empty
            };


            var initSuccess = await _daemonState.Initialize();
            if (!initSuccess)
            {
                result.IsSuccessful = false;
                result.Status = $"Unable to initialize daemon state on machine {Environment.MachineName}";
                result.CalculateTimeSpent();

                return result;
            }

            var fulfillContractsResult = await _daemonService.ProcessPendingDaemonContracts();


            
        }
    }
}
