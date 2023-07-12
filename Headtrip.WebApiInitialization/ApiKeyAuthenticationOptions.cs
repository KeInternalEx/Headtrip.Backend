using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string? SchemeName { get; set; }
        public string? JwtSchemeName { get; set; }
        public string? KeyName { get; set; }
        
    }
}
