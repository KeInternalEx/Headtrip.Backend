using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Account
{
    public sealed class MAccount : ADatabaseObject, IDatabaseObjectTimeStamped, IDatabaseObjectDelayedDeletable
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }


        public DateTime LastLoginTime { get; set; }

        public long TotalPlaytimeMs { get; set; }
        public long Money { get; set; }

        public bool IsLocked { get; set; }
        public bool IsSuspended { get; set; }


        public bool IsPendingDeletion { get; set; }
        public DateTime? DateDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
