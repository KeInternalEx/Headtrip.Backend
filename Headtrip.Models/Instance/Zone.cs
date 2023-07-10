using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    public class Zone
    {
        public string ZoneName { get; set; } = null!; // The name of the zone, this field is UNIQUE
        public string LevelName { get; set; } = null!; // The name of the level that needs to be passed to the unreal engine instance


    }
}
