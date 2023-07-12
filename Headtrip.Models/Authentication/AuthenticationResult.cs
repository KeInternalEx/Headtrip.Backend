using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Authentication.Models
{
    public class AuthenticationResult : ServiceCallResult
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
