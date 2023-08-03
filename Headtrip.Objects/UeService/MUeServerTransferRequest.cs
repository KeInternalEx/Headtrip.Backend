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
        Unknown = 0,
        
        PendingTransform = 10,
        
        PendingServerCreation = 20,
        FailedServerCreation = 21,


        PendingAssignment = 30,
        FailedAssignment = 31,

        Completed = 255
    }


    public sealed class MUeServerTransferRequest : ADatabaseObject
    {
        public Guid UeServerTransferRequestId { get; set; }

        public Guid CurrentChannelId { get; set; }
        public Guid CurrentUeServiceId { get; set; }
        
        public Guid? TargetChannelId { get; set; }
        public Guid? TargetUeServiceId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid AccountId { get; set; }
        public Guid? PartyId { get; set; }

        public string? ZoneName { get; set; }
        public int CharacterLevel { get; set; }

        public EUeServerTransferRequestState State { get; set; }
        

    }
}
