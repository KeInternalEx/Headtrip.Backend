using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : 
            base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(JwtConfiguration.JwtHeaderName, out var authHeader) ||
                !AuthenticationHeaderValue.TryParse(authHeader, out var parsedAuthHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (parsedAuthHeader.Scheme != Options.SchemeName)
                return AuthenticateResult.NoResult();

            if (string.IsNullOrWhiteSpace(parsedAuthHeader.Parameter))
                return AuthenticateResult.Fail("MISSING JWT");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(parsedAuthHeader.Parameter, new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                RequireAudience = true,
                ValidateIssuer = true,

                ValidAudience = JwtConfiguration.JwtAudience,
                ValidIssuer = JwtConfiguration.JwtIssuer,
                
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.JwtSigningKey)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.JwtEncryptionKey)),
            });

            if (tokenValidationResult.IsValid)
            {
                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new System.Security.Claims.ClaimsPrincipal(tokenValidationResult.ClaimsIdentity),
                        Options.SchemeName));
            }

            return AuthenticateResult.Fail("INVALID JWT");
        }
    }
}
