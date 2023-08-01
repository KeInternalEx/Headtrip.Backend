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
    public sealed class MUeService : ADatabaseObject, IDatabaseObjectTimeStamped, IDatabaseObjectDelayedDeletable
    {
        public Guid UeServiceId { get; set; }
        public string? Nickname { get; set; }
        public string? ServerAddress { get; set; }

        public int NumberOfFreeEntries { get; set; }
        public bool IsSuperService { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsPendingDeletion { get; set; }


    }
}
