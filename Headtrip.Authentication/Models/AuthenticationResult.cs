using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Authentication.Models
{
    public class AuthenticationResult
    {
        public Guid AccountId { get; set; }
        public Guid SessionId { get; set; }
    }
}
