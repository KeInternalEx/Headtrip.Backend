﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.UeService
{
    public sealed class TStrGroup
    {
        public string? ZoneName { get; set; }
        public Guid UeServiceId { get; set; }
        public Guid GroupId { get; set; }
        public List<MUeServerTransferRequest>? ServerTransferRequests { get; set; }

        public byte NumberOfParties { get; set; }
        public byte NumberOfPlayers { get; set; }

    }
}