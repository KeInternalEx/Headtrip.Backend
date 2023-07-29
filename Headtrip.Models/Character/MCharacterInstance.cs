using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Character
{
    public sealed class MCharacterInstance : ADatabaseObject
    {
        public Guid CharacterId { get; set; }
        public Guid ClassId { get; set; }
        public string? Name { get; set; }
        public long Experience { get; set; }
        
    }
}
