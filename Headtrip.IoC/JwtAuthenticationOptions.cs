using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string? SchemeName { get; set; }
    }
}
