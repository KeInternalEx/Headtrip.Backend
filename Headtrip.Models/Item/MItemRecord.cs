using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Item
{
    public sealed class MItemRecord : ADatabaseObject
    {
        public Guid ItemRecordId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string FlavorText { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Icon { get; set; } = null!;

        public int PossibleRarityBitmap { get; set; }


        // These records will be updated every hour by a service
        public long AveragePrice { get; set; }
        public long MaximumPrice { get; set; }
        public long MinimumPrice { get; set; }



        public bool IsEquippable { get; set; }
        public bool IsConsumable { get; set; }
        public bool IsDeprecated { get; set; }



        
    }
}
