﻿using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.User
{
    public class UserIdDecryptionResult : AServiceCallResult
    {
        public Guid UserId { get; set; }
    }
}
