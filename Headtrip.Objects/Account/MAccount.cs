using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Account
{
    public sealed class MAccount : ADatabaseObject, IDatabaseObjectTimeStamped, IDatabaseObjectDelayedDeletable
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }

        public Guid? CurrentCharacterId { get; set; }
        public Guid? CurrentChannelId { get; set; }
        public Guid? CurrentPartyId { get; set; }

        public DateTime LastLoginTime { get; set; }
        public long TotalPlayTimeMs { get; set; }

        public bool IsLocked { get; set; }
        public bool IsSuspended { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public bool IsPendingDeletion { get; set; }

        public override void MapToRow(ref DataRow Row)
        {
            Row["AccountId"] = AccountId;
            Row["UserId"] = UserId;

            Row["CurrentCharacterId"] = CurrentCharacterId;
            Row["CurrentChannelId"] = CurrentChannelId;
            Row["CurrentPartyId"] = CurrentPartyId;

            Row["LastLoginTime"] = LastLoginTime;
            Row["TotalPlayTimeMs"] = TotalPlayTimeMs;
            Row["IsLocked"] = IsLocked;
            Row["IsSuspended"] = IsSuspended;
            Row["DateCreated"] = DateCreated;
            Row["DateModified"] = DateModified;
            Row["DateDeleted"] = DateDeleted;
            Row["IsPendingDeletion"] = IsPendingDeletion;
        }

        public override void MapToColumns(DataColumnCollection Columns)
        {
            Columns.Add(new DataColumn("AccountId", typeof(Guid)));
            Columns.Add(new DataColumn("UserId", typeof(Guid)));
            Columns.Add(new DataColumn("CurrentCharacterId", typeof(Guid?)));
            Columns.Add(new DataColumn("CurrentChannelId", typeof(Guid?)));
            Columns.Add(new DataColumn("CurrentPartyId", typeof(Guid?)));
            Columns.Add(new DataColumn("LastLoginTime", typeof(DateTime)));
            Columns.Add(new DataColumn("IsLocked", typeof(bool)));
            Columns.Add(new DataColumn("IsSuspended", typeof(bool)));
            Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
            Columns.Add(new DataColumn("DateModified", typeof(DateTime?)));
            Columns.Add(new DataColumn("DateDeleted", typeof(DateTime?)));
            Columns.Add(new DataColumn("IsPendingDeletion", typeof(bool)));
        }

    }
}
