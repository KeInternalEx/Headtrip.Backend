using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    [SqlTable("Zones", "GS")]
    public class Zone : DatabaseObject
    {
        [SqlColumn(type ="nvarchar(255)", primaryKey =true,unique =true,index ="zonename")]
        public string? ZoneName { get; set; } = null!; // The name of the zone, this field is UNIQUE
        [SqlColumn(type ="nvarchar(max)", unique =true)]
        public string? LevelName { get; set; } = null!; // The name of the level that needs to be passed to the unreal engine instance

        [SqlColumn(type ="int")]
        public int SoftPlayerCap { get; set; }

        [SqlColumn(type ="int")]
        public int HardPlayerCap { get; set; }

    }
}
