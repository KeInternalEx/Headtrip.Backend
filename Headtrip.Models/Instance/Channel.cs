using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    public class Channel
    {
        public Guid ChannelId { get; set; }
        public string ZoneName { get; set; } = null!;
        public bool IsHardcore { get; set; }
        public bool IsPvpForced { get; set; }
        public DateTime CreationDate { get; set; }
        public string GameInstance { get; set; } = null!;
    }
}
