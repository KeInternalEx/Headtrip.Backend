using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonContract : DatabaseObject
    {
        public Guid DaemonContractId { get; set; }
        public Guid CurrentChannelId { get; set; }
        public Guid CurrentDaemonId { get; set; }
        public Guid? TargetChannelId { get; set; }
        public Guid? TargetDaemonId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? PartyId { get; set; }
        public string? ZoneName { get; set; }
        public bool IsProcessing { get; set; }

        

    }
}
