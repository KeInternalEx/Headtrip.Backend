using BCrypt.Net;
using Headtrip.Authentication.Models;
using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Models.Abstract;
using Headtrip.Models.User;
using Headtrip.Repositories.Abstract;
using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Headtrip.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly int EXPIRATION_DAYS_GAMESERVER = 1;
        private static readonly int EXPIRATION_DAYS_WEBSITE = 7;


        private readonly ILogging<HeadtripLoginServerContext> _logging;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork<HeadtripLoginServerContext> _unitOfWork;

        public AuthenticationService(
            ILogging<HeadtripLoginServerContext> logging,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IUnitOfWork<HeadtripLoginServerContext> unitOfWork)
        {
            _logging = logging;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }


        private List<Claim> createDefaultClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, GameServerJwtConfiguration.JwtSubject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Nonce
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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
                IsSuccessful = true,
                Status = string.Empty,

                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            };

        }

        private AuthenticationResult AuthenticateUser(
            User user,
            string password)
        {

            var authenticationSuccess = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash, false, HashType.SHA384);
            if (authenticationSuccess)
            {
                var expiration = DateTime.UtcNow.AddDays(EXPIRATION_DAYS_WEBSITE);
                var claims = createDefaultClaims(user);

                return CreateAuthenticationToken(claims, expiration);
            }


            throw new Exception($"Authentication failed for user {user.Username}");
        }


        private async Task<AuthenticationResult> AuthenticateUserForGameServer(
            User user,
            string password)
        {

            var authenticationSuccess = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash, false, HashType.SHA384);
            if (authenticationSuccess)
            {
                var expiration = DateTime.UtcNow.AddDays(EXPIRATION_DAYS_GAMESERVER);
                var claims = createDefaultClaims(user);

                // TODO: GET ACCOUNT ID, GET/CREATE SESSION ID
                // TODO: APPEND GAME SERVER CLAIMS

                return CreateAuthenticationToken(claims, expiration);
            }


            throw new Exception($"Game Server Authentication failed for user {user.Username}");
        }

        public async Task<AuthenticationResult> AuthenticateUserByUsername(
            string username,
            string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUsername(username);
                if (user == null)
                    return new AuthenticationResult { IsSuccessful = false, Status = $"No user found for Username {username}" };

                return AuthenticateUser(user, password);
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return ServiceCallResult.BuildForException<AuthenticationResult>(ex);
            }
        }

        public async Task<AuthenticationResult> AuthenticateUserByEmail(
            string email,
            string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                    return new AuthenticationResult { IsSuccessful = false, Status = $"No user found for Email {email}" };

                return AuthenticateUser(user, password);
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return ServiceCallResult.BuildForException<AuthenticationResult>(ex);
            }
        }


        public async Task<AuthenticationResult> AuthenticateUserByUsernameForGameServer(
            string username,
            string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUsername(username);
                if (user == null)
                    return new AuthenticationResult { IsSuccessful = false, Status = $"No user found for Username {username}" };

                return await AuthenticateUserForGameServer(user, password);
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return ServiceCallResult.BuildForException<AuthenticationResult>(ex);
            }
        }

        public async Task<AuthenticationResult> AuthenticateUserByEmailForGameServer(
            string email,
            string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                    return new AuthenticationResult { IsSuccessful = false, Status = $"No user found for Email {email}" };

                return await AuthenticateUserForGameServer(user, password);
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return ServiceCallResult.BuildForException<AuthenticationResult>(ex);
            }
        }


      
    }
}
