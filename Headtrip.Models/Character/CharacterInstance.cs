using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Character
{
    public class CharacterInstance
    {
        public Guid CharacterId { get; set; }
        public Guid ClassId { get; set; }

        public string Name { get; set; }

        public long Experience { get; set; }
        
    }
}
