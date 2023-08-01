using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.UeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Objects
{
    public sealed class RCreateServerTransferRequestGroupsResult : AServiceCallResult
    {
        public List<TUeServiceServerTransferRequestGroup>? RequestGroups { get; set; }

    }
}
