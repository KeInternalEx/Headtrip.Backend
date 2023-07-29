using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Database
{
    /**
     * SQL COLUMNS
     ** IsPendingDeletion BIT NOT NULL DEFAULT 0
     ** DateDeleted DATETIME2 NULL
     */
    public interface IDatabaseObjectDelayedDeletable
    {
        public bool IsPendingDeletion { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
