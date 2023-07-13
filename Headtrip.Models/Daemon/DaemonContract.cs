using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    [SqlTable("DaemonContracts", "GS")]
    public class DaemonContract : DatabaseObject
    {
        [SqlColumn(type ="UniqueIdentifier")]
        public Guid CurrentChannelId { get; set; }
        [SqlColumn(type ="UniqueIdentifier")]
        public Guid CurrentDaemonId { get; set; }

        [SqlColumn(type ="UniqueIdentifier", nullable =true)]
        public Guid? TargetChannelId { get; set; }
        [SqlColumn(type ="UniqueIdentifier", nullable =true)]
        public Guid? TargetDaemonId { get; set; }


        [SqlColumn(type ="UniqueIdentifier")]
        public Guid AccountId { get; set; }

        [SqlColumn(type = "UniqueIdentifier", nullable = true)]
        public Guid? PartyId { get; set; }

        [SqlColumn(type = "nvarchar(255)")]
        public string? ZoneName { get; set; }


        [SqlColumn(type ="bit", defaultValue = false)]
        public bool IsProcessing { get; set; }

        [SqlColumn(type ="bit", defaultValue = false)]
        public bool IsTransforming { get; set; }

    }
}
