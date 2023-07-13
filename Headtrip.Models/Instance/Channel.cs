using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    [SqlTable("Channels", "GS")]
    public class Channel : DatabaseObject
    {
        [SqlColumn(index ="channelid", primaryKey =true,unique =true,type ="UniqueIdentifier")]
        public Guid ChannelId { get; set; }
        [SqlColumn(index ="channeldaemonId",type ="UniqueIdentifier")]
        public Guid DaemonId { get; set; }

        [SqlColumn(type ="nvarchar(255)")]
        public string? ZoneName { get; set; }

        [SqlColumn(type ="nvarchar(max)")]
        public string? ConnectionString { get; set; }

        [SqlColumn(type ="bit", defaultValue =false)]
        public bool IsAvailable { get; set; } // Set to false while spin up is in process, set to true once pending contracts have been executed
    }
}
