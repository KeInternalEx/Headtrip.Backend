using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Account
{

    [SqlTable("Accounts", "GS")]
    public class Account : DatabaseObject
    {
        [SqlColumn(index = "accountId", primaryKey = true, unique = true, type = "UniqueIdentifier")]
        public Guid AccountId { get; set; }

        [SqlColumn(unique = true, type = "UniqueIdentifier")]
        public Guid UserId { get; set; }

        [SqlColumn(type = "datetime2")]
        public DateTime LastLoginTime { get; set; }

        [SqlColumn(type = "bigint", defaultValue = 0)]
        public long TotalPlaytimeMs { get; set; }

        [SqlColumn(type = "bigint", defaultValue = 0)]
        public long Money { get; set; }

        [SqlColumn(type = "datetime2")]
        public DateTime CreatedOn { get; set; }

        [SqlColumn(type = "datetime2")]
        public DateTime LastModifiedOn { get; set; }

    }
}
