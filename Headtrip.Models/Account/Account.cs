using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Account
{
    public class Account : DatabaseObject
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LastLoginTime { get; set; }
        public long TotalPlaytimeMs { get; set; }
        public long Money { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public bool IsMarkedForDeletion { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSuspended { get; set; }

    }
}
