
using Headtrip.UeService.Models;
using Headtrip.UeService.Services.Abstract;
using Headtrip.UeService.Tasks.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks
{
    public sealed class ServerTransferRequestTansformationTask : IUeServiceTask
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IUeServiceTaskService _UeServiceTaskService;


        public ServerTransferRequestTansformationTask(
            ILogging<HeadtripGameServerContext> logging,
            IUeServiceTaskService UeServiceTaskService)
        {
            _logging = logging;
            _UeServiceTaskService = UeServiceTaskService;
        }



        public async Task<UeServiceTaskResult> Execute()
        {
            var getUeServiceServerTransferRequestGroupsResult = await _UeServiceTaskService.GetUeServiceServerTransferRequestGroups();
            if (getUeServiceServerTransferRequestGroupsResult.IsSuccessful && getUeServiceServerTransferRequestGroupsResult.ServerTransferRequestGroups.Count > 0)
            {




            }

             

        }
    }
}
