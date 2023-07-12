using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Headtrip.WebApiInitialization
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {

        public static async Task<TokenValidationResult> ValidateJwt(AuthenticationHeaderValue parsedAuthHeader)
        {
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

            return tokenValidationResult;
        }

        public static List<Claim> ExtractJwtClaims(TokenValidationResult tokenValidationResult) =>
            tokenValidationResult.Claims.Select((kv) => new Claim(kv.Key, kv.Value.ToString() ?? "")).ToList();

        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : 
            base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (
                string.IsNullOrEmpty(Options.SchemeName) ||
                !AuthenticationHandlerUtilities.GetAuthenticationHeader(Request, JwtConfiguration.JwtHeaderName, Options.SchemeName, out var parsedAuthHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (parsedAuthHeader == null || string.IsNullOrWhiteSpace(parsedAuthHeader.Parameter))
                return AuthenticateResult.Fail("MISSING JWT");

            var tokenValidationResult = await ValidateJwt(parsedAuthHeader);

            if (tokenValidationResult.IsValid)
            {
                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(new ClaimsIdentity(ExtractJwtClaims(tokenValidationResult), Options.SchemeName)),
                        Options.SchemeName));
            }

            return AuthenticateResult.Fail("INVALID JWT");
        }
    }
}
