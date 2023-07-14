using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    public class Channel : DatabaseObject
    {
        public Guid ChannelId { get; set; }
        public Guid DaemonId { get; set; }
        public string? ZoneName { get; set; }
        public string? ConnectionString { get; set; }
        public bool IsAvailable { get; set; } // Set to false while spin up is in process, set to true once pending contracts have been executed
    }
}
