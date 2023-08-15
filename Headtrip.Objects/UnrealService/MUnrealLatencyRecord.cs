using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UnrealService
{
    public sealed class MUnrealLatencyRecord : ADatabaseObject
    {
        public Guid UnrealLatencyRecordId { get; set; }
        public Guid UnrealServiceId { get; set; }
        public Guid AccountId { get; set; }
        public float Latency { get; set; }

        public override void MapToRow(ref DataRow Row)
        {
            Row["UnrealLatencyRecordId"] = UnrealLatencyRecordId;
            Row["UnrealServiceId"] = UnrealServiceId;
            Row["AccountId"] = AccountId;
            Row["Latency"] = Latency;
        }

        public override void MapToColumns(DataColumnCollection Columns)
        {
            Columns.Add(new DataColumn("UnrealLatencyRecordId", typeof(Guid)));
            Columns.Add(new DataColumn("UnrealServiceId", typeof(Guid)));
            Columns.Add(new DataColumn("AccountId", typeof(Guid)));
            Columns.Add(new DataColumn("Latency", typeof(float)));
        }

    }
}
