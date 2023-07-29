using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Headtrip.Objects.UeService
{
    public sealed class MUeService : ADatabaseObject
    {
        public Guid UeServiceId { get; set; }
        public string? Nickname { get; set; }
        public string? ServerAddress { get; set; }

        public int NumberOfFreeEntries { get; set; }
        public bool IsSuperUeService { get; set; }
    }
}
