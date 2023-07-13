using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Headtrip.Models.Daemon
{
    [SqlTable("Daemons", "GS")]
    public class Daemon : DatabaseObject
    {

        [SqlColumn(primaryKey = true, unique = true, type = "UniqueIdentifier")]
        public Guid DaemonId { get; set; }

        [SqlColumn(type = "nvarchar(max)")]
        public string? Nickname { get; set; }
    }
}
