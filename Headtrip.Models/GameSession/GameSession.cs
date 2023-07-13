using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.GameSession
{
    [SqlTable("GameSessions", "GS")]
    public class GameSession : DatabaseObject
    {
        [SqlColumn(index = "gamesessionid", primaryKey = true, unique = true, type = "UniqueIdentifier")]
        public Guid GameSessionId { get; set; }

        [SqlColumn(index = "gamesessionaccountid", foreignKey ="Account.AccountId", type ="UniqueIdentifier", unique = true)]
        public Guid AccountId { get; set; }

        [SqlColumn(type ="UniqueIdentifier", nullable =true, defaultValue =null)]
        public Guid? SelectedCharacterId { get; set; }

        [SqlColumn(type ="UniqueIdentifier", nullable =true, defaultValue =null)]
        public Guid? PartyId { get; set; }

        [SqlColumn(type ="int", nullable=true, defaultValue = null)]
        public int? PartySlot { get; set; }

    }
}
