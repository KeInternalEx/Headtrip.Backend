using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Item
{
    public sealed class MItemInstance : ADatabaseObject, IDatabaseObjectTimeStamped, IDatabaseObjectDelayedDeletable
    {
        public Guid ItemInstanceId { get; set; }
        public Guid ItemRecordId { get; set; }

        public string? UserDefinedName { get; set; } = null!;

        public int Amount { get; set; }
        public bool IsIdentified { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsPendingDeletion { get; set; }
    }
}
