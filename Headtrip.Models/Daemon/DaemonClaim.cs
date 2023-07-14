﻿using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Daemon
{
    public class DaemonClaim : DatabaseObject
    {
        public string? ZoneName { get; set; }
        public Guid DaemonId { get; set; }
        public byte NumberOfContracts { get; set; }
    }
}
