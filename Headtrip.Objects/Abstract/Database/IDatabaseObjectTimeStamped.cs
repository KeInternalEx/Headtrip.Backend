using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Database
{
    /**
     * SQL COLUMNS
     ** DateCreated DATETIME2 NOT NULL
     ** DateModified DATETIME2 NULL
     */
    public interface IDatabaseObjectTimeStamped
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
