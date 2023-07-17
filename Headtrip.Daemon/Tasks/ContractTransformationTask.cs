﻿
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
            var getDaemonContractGroupsResult = await _daemonService.GetDaemonContractGroups();
            if (getDaemonContractGroupsResult.IsSuccessful && getDaemonContractGroupsResult.ContractGroups.Count > 0)
            {




            }

             

        }
    }
}
