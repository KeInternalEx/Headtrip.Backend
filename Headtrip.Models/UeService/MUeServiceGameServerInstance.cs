using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class MUeServiceGameServerInstance : ADatabaseObject
    {
        public Guid UeServiceGameServerInstanceId { get; set; }
        public Guid UeServiceId { get; set; }
        public string? ConnectionString { get; set; }
        public byte NumberOfPlayersConnected { get; set; }

    }
}
