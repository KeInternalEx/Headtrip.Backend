using Headtrip.Objects.Abstract.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Authentication.Models
{
    public sealed class RLoginResult : AServiceCallResult
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
