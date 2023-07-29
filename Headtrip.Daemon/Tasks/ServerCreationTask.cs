
using Headtrip.UeService.Models;
using Headtrip.UeService.Services.Abstract;
using Headtrip.UeService.State.Abstract;
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
    public sealed class ServerCreationTask : IUeServiceTask
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IUeServiceState _UeServiceState;
        private readonly IUeServiceTaskService _UeServiceService;


        public ServerCreationTask(
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




            // var claims = await 





            // TODO: THEN THE SERVICE WILL PICK UP THE ServerTransferRequestS IT HAS A "TRANSFORMING ID" MATCHING
            // TODO: IT WILL COLLECT THE CHANNELS BASED ON THOSE ServerTransferRequestS AND IT WILL CREATE AN UNREAL SERVER FOR EACH CHANNEL
            // TODO: IT WILL UPDATE THE CHANNEL RECORDS WITH THE CONNECTION STRINGS FOR THE UNREAL SERVERS
            // TODO: IT WILL THEN UPDATE THE ServerTransferRequestS WITH THE UeServiceId, ALLOWING IT TO BE PICKED UP BY THE CORRECT UeServiceS

            return null;

        }
    }
}
