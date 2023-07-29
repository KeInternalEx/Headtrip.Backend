
using Headtrip.UeService.Models;
using Headtrip.UeService.Services.Abstract;
using Headtrip.UeService.State.Abstract;
using Headtrip.UeService.Tasks.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks
{
    public sealed class ServerTransferRequestFulfillmentTask : IUeServiceTask
    {

        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IUeServiceState _UeServiceState;
        private readonly IUeServiceTaskService _UeServiceService;


        public ServerTransferRequestFulfillmentTask(
            ILogging<HeadtripGameServerContext> logging,
            IUeServiceState UeServiceState,
            IUeServiceTaskService UeServiceService)
        {
            _logging = logging;
            _UeServiceState = UeServiceState;
            _UeServiceService = UeServiceService;
        }


        public async Task<UeServiceTaskResult> Execute()
        {
            var result = new UeServiceTaskResult
            {
                IsSuccessful = false,
                Status = string.Empty
            };


            var initSuccess = await _UeServiceState.Initialize();
            if (!initSuccess)
            {
                result.IsSuccessful = false;
                result.Status = $"Unable to initialize UeService state on machine {Environment.MachineName}";
                result.CalculateTimeSpent();

                return result;
            }

            var fulfillServerTransferRequestsResult = await _UeServiceService.ProcessPendingUeServiceServerTransferRequests();


            return null;
        }
    }
}
