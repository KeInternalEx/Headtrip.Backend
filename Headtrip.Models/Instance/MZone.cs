using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Instance
{
    public sealed class MZone : ADatabaseObject
    {
        public string? ZoneName { get; set; } = null!; // The name of the zone, this field is UNIQUE
        public string? LevelName { get; set; } = null!; // The name of the level that needs to be passed to the unreal engine instance
        public byte SoftPlayerCap { get; set; }
        public byte HardPlayerCap { get; set; }

    }
}
