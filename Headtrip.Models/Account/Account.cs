using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Account
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public Guid UserId { get; set; }


        public DateTime LastLoginTime { get; set; }
        public DateTime CreationTime { get; set; }

        public long TotalPlaytimeMs { get; set; }

        public string InventoryArray { get; set; }
        public long Money { get; set; }
       


    }
}
