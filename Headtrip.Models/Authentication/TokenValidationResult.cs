using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Authentication
{
    public class TokenValidationResult : ServiceCallResult
    {
        public bool IsTokenValid { get; set; }
    }
}
