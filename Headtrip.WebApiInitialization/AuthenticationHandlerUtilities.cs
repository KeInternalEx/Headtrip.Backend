using Azure.Core;
using Headtrip.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public static class AuthenticationHandlerUtilities
    {

        public static bool GetAuthenticationHeader(
            HttpRequest request,
            string headerName,
            string schemeName,
            out AuthenticationHeaderValue? authenticationHeader)
        {
            authenticationHeader = null;

            if (!request.Headers.TryGetValue(headerName, out var authHeader) ||
                !AuthenticationHeaderValue.TryParse(authHeader, out var parsedAuthHeader))
            {
                return false;
            }

            if (parsedAuthHeader.Scheme != schemeName)
                return false;


            authenticationHeader = parsedAuthHeader;
            return true;
        }

    }
}
