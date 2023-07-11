using Headtrip.Authentication.Models;
using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Repositories.Abstract;
using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly int EXPIRATION_DAYS_GAMESERVER = 1;
        private static readonly int EXPIRATION_DAYS_WEBSITE = 7;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext> _unitOfWork;

        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            IAccountRepository accountRepository,
            IUnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext> unitOfWork)
        {
            _userManager = userManager;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }


        private List<Claim> createDefaultClaims(IdentityUser identityUser)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, GameServerJwtConfiguration.JwtSubject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Nonce
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
            };
        }

   
        private AuthenticationResult CreateAuthenticationToken(
            IEnumerable<Claim> claims,
            DateTime expiration)
        {

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GameServerJwtConfiguration.JwtSigningKey)
                ),
                SecurityAlgorithms.HmacSha256Signature
            );

            var encryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GameServerJwtConfiguration.JwtEncryptionKey)
                ),
                SecurityAlgorithms.Aes128Encryption
            );

            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
   

            var token = tokenHandler.CreateJwtSecurityToken(
                GameServerJwtConfiguration.JwtIssuer,
                GameServerJwtConfiguration.JwtAudience,
                claimsIdentity,
                DateTime.UtcNow,
                expiration,
                DateTime.UtcNow,
                signingCredentials,
                encryptingCredentials);


            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            };

        }

        private async Task<AuthenticationResult> AuthenticateUser(
            IdentityUser identityUser,
            string password)
        {

            var authenticationSuccess = await _UserManager.CheckPasswordAsync(identityUser, password);
            if (authenticationSuccess)
            {
                var expiration = DateTime.UtcNow.AddDays(EXPIRATION_DAYS_WEBSITE);
                var claims = createDefaultClaims(identityUser);

                return CreateAuthenticationToken(claims, expiration);
            }


            throw new Exception($"Authentication failed for user {identityUser.UserName}");
        }


        private async Task<AuthenticationResult> AuthenticateUserForGameServer(
            IdentityUser identityUser,
            string password)
        {

            var authenticationSuccess = await _UserManager.CheckPasswordAsync(identityUser, password);
            if (authenticationSuccess)
            {
                var expiration = DateTime.UtcNow.AddDays(EXPIRATION_DAYS_GAMESERVER);
                var claims = createDefaultClaims(identityUser);

                // TODO: GET ACCOUNT ID, GET/CREATE SESSION ID
                // TODO: APPEND GAME SERVER CLAIMS

                return CreateAuthenticationToken(claims, expiration);
            }


            throw new Exception($"Game Server Authentication failed for user {identityUser.UserName}");
        }

        public async Task<AuthenticationResult> AuthenticateUserByUsername(
            string username,
            string password)
        {
            var identityUser = await _UserManager.FindByNameAsync(username);
            if (identityUser == null)
                throw new Exception($"Authentication failed for user {username}");

            return await AuthenticateUser(identityUser, password);
        }

        public async Task<AuthenticationResult> AuthenticateUserByEmail(
            string email,
            string password)
        {
            var identityUser = await _UserManager.FindByEmailAsync(email);
            if (identityUser == null)
                throw new Exception($"Authentication failed for email {email}");

            return await AuthenticateUser(identityUser, password);
        }


        public async Task<AuthenticationResult> AuthenticateUserByUsernameForGameServer(
            string username,
            string password)
        {
            var identityUser = await _UserManager.FindByNameAsync(username);
            if (identityUser == null)
                throw new Exception($"Game Server Authentication failed for user {username}");

            return await AuthenticateUserForGameServer(identityUser, password);
        }

        public async Task<AuthenticationResult> AuthenticateUserByEmailForGameServer(
            string email,
            string password)
        {
            var identityUser = await _UserManager.FindByEmailAsync(email);
            if (identityUser == null)
                throw new Exception($"Game Server Authentication failed for email {email}");

            return await AuthenticateUserForGameServer(identityUser, password);
        }

    }
}
