using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public enum EUeServerTransferRequestState : byte
    {
        Completed = 0,
        PendingTransform = 1,
        Transforming = 2,
        PendingAssignment = 3,
        Assigning = 4
    }


    public sealed class MUeServerTransferRequest : ADatabaseObject
    {
        public Guid UeServerTransferRequestId { get; set; }

        public Guid CurrentChannelId { get; set; }
        public Guid CurrentUeServiceId { get; set; }
        
        public Guid? TargetChannelId { get; set; }
        public Guid? TargetUeServiceId { get; set; }


        public Guid AccountId { get; set; }
        public Guid? PartyId { get; set; }

        public string? ZoneName { get; set; }
        public int CharacterLevel { get; set; }

        public EUeServerTransferRequestState State { get; set; }
        

    }
}
