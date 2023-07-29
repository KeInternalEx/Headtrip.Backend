using Headtrip.Objects.Abstract.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class RGetUeServiceServerTransferRequestGroupsResult : AServiceCallResult
    {
        public List<UeServiceServerTransferRequestGroup>? ServerTransferRequestGroups { get; set; }
    }
}
