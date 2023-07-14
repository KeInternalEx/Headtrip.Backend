using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Item
{
    public class ItemInstance : DatabaseObject
    {
        public Guid ItemInstanceId { get; set; }
        public Guid ItemRecordId { get; set; }
        public string? UserDefinedName { get; set; } = null!;
        public int Amount { get; set; }
        public bool Identified { get; set; }
    }
}
