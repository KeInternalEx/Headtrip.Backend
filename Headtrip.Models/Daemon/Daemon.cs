using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Headtrip.Models.Daemon
{
    public class Daemon : DatabaseObject
    {
        public Guid DaemonId { get; set; }
        public string? Nickname { get; set; }
        public int NumberOfFreeEntries { get; set; }
    }
}
