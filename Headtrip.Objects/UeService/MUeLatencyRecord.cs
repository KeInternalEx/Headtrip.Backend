using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class MUeLatencyRecord : ADatabaseObject
    {
        public Guid UeLatencyRecordId { get; set; }
        public Guid UeServiceId { get; set; }
        public Guid AccountId { get; set; }
        public float Latency { get; set; }

        public override void MapToRow(ref DataRow Row)
        {
            Row["UeLatencyRecordId"] = UeLatencyRecordId;
            Row["UeServiceId"] = UeServiceId;
            Row["AccountId"] = AccountId;
            Row["Latency"] = Latency;
        }

        public override void MapToColumns(DataColumnCollection Columns)
        {
            Columns.Add(new DataColumn("UeLatencyRecordId", typeof(Guid)));
            Columns.Add(new DataColumn("UeServiceId", typeof(Guid)));
            Columns.Add(new DataColumn("AccountId", typeof(Guid)));
            Columns.Add(new DataColumn("Latency", typeof(float)));
        }

    }
}
