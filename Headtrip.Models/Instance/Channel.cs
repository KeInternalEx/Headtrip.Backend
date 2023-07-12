﻿using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Instance
{
    public class Channel : DatabaseObject
    {
        public Guid ChannelId { get; set; }
        public string? ZoneName { get; set; } = null!;
        public string? ConnectionString { get; set; } = null!;
    }
}
