using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    [SqlTable("DaemonClaims", "GS")]
    public class DaemonClaim : DatabaseObject
    {
        [SqlColumn(type ="nvarchar(255)", index ="daemonclaim_zonename")]
        public string? ZoneName { get; set; }

        [SqlColumn(type = "UniqueIdentifier")]
        public Guid DaemonId { get; set; }
    }
}
