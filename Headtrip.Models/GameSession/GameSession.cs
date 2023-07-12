using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.GameSession
{
    public class GameSession : DatabaseObject
    {
        public Guid GameSessionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CharacterId { get; set; }
        public Guid? PartyId { get; set; }
        public int? PartySlot { get; set; }

    }
}
