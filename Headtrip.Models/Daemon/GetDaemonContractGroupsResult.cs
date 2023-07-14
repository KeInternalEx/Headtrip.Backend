using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class GetDaemonContractGroupsResult : ServiceCallResult
    {
        public List<DaemonContractGroup>? ContractGroups { get; set; }
    }
}
