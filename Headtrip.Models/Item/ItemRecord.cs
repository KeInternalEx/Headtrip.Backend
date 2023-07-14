using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Item
{
    public class ItemRecord : DatabaseObject
    {
        public Guid ItemRecordId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string FlavorText { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Icon { get; set; } = null!;

        public int PossibleRarityBitmap { get; set; }
        
        public bool Equippable { get; set; }
        public bool Consumable { get; set; }

        public long AveragePrice { get; set; }

        
    }
}
