using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Character
{
    public sealed class MCharacter : ADatabaseObject, IDatabaseObjectDelayedDeletable, IDatabaseObjectTimeStamped
    {
        public Guid CharacterId { get; set; }
        public Guid ClassId { get; set; }
        public string? Name { get; set; }
        public long Experience { get; set; }
        

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsPendingDeletion { get; set; }
    }
}
