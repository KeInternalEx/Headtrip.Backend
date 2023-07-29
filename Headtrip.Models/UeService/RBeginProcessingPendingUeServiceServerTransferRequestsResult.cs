using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class RBeginProcessingPendingUeServiceServerTransferRequestsResult : AServiceCallResult
    {
        public List<MUeServiceServerTransferRequest>? ServerTransferRequests { get; set; }
        public List<MChannel>? Channels { get; set; }
    }
}
