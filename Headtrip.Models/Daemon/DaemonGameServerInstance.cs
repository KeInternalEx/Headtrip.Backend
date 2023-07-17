using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonGameServerInstance : DatabaseObject
    {
        public Guid DaemonGameServerInstanceId { get; set; }
        public Guid DaemonId { get; set; }
        public string? ConnectionString { get; set; }
        public byte NumberOfPlayersConnected { get; set; }

    }
}
