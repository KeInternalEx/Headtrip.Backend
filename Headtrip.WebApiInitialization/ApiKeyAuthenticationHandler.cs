using Headtrip.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (
                string.IsNullOrEmpty(Options.SchemeName) ||
                !AuthenticationHandlerUtilities.GetAuthenticationHeader(Request, InternalApiKeys.ApiKeyHeaderName, Options.SchemeName, out var parsedAuthHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (parsedAuthHeader == null || string.IsNullOrWhiteSpace(parsedAuthHeader.Parameter))
                return AuthenticateResult.Fail("MISSING API KEY");

            if(!string.IsNullOrEmpty(Options.KeyName) && parsedAuthHeader.Parameter == InternalApiKeys.GetApiKey(Options.KeyName))
            {
                if (!string.IsNullOrEmpty(Options.JwtSchemeName) && // We want to attempt to load claims from a JWT Token also
                    AuthenticationHandlerUtilities.GetAuthenticationHeader(Request, JwtConfiguration.JwtHeaderName, Options.JwtSchemeName, out var jwtAuthHeader) &&
                    jwtAuthHeader != null)
                {
                    // We have a JWT, if it's invalid at this point we need to signal a failure because that means
                    // The client passed the UnrealGameServer an invalid JWT
                    // Which means that the UnrealGameServer needs to know that it's invalid and disconnect the client for either playing for too long (potential bot)
                    // Or for being a stinky cheater and trying use someone else's jwt or modify their own

                    var tokenValidationResult = await JwtAuthenticationHandler.ValidateJwt(jwtAuthHeader);
                    if (tokenValidationResult.IsValid)
                    {
                        return AuthenticateResult.Success(
                            new AuthenticationTicket(
                                new ClaimsPrincipal(new ClaimsIdentity(JwtAuthenticationHandler.ExtractJwtClaims(tokenValidationResult), Options.SchemeName)),
                                Options.SchemeName));
                    }

                    return AuthenticateResult.Fail("INVALID JWT");
                }

                // If we get here that means that we either aren't extracting the claims from a JWT or we didn't have a header for this request.
                // So we should proceed as normal.

                return AuthenticateResult.Success(new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, Options.SchemeName)),
                    Options.SchemeName));
            }

            return AuthenticateResult.Fail("INVALID API KEY");
        }
    }
}
