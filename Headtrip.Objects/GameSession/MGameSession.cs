using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.GameSession
{
    public sealed class MGameSession : ADatabaseObject
    {
        public Guid GameSessionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? SelectedCharacterId { get; set; }
        public Guid? PartyId { get; set; }
        public int? PartySlot { get; set; }

    }
}
