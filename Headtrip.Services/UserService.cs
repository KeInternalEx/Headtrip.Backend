using BCrypt.Net;
using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.Account;
using Headtrip.Objects.Account.Result;
using Headtrip.Objects.User;
using Headtrip.Objects.User.Result;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.Repositories.Repositories.Interface.LoginServer;
using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Interface;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Headtrip.Services
{

    public sealed class UserService : IUserService
    {
        private readonly ILogging<HeadtripLoginServerContext> _logging;

        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        private readonly IContext<HeadtripGameServerContext> _GsContext;
        private readonly IContext<HeadtripLoginServerContext> _LsContext;


        public UserService(
            ILogging<HeadtripLoginServerContext> logging,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IContext<HeadtripGameServerContext> GsContext,
            IContext<HeadtripLoginServerContext> LsContext)
        {
            _logging = logging;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _GsContext = GsContext;
            _LsContext = LsContext;
        }


        private static string EncryptStringWeak(string s) 
        {
            var key = Encoding.UTF8.GetBytes(InternalEncryptionKeys.EmailConfirmationKey);
            var iv = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            using (var aes = Aes.Create())
            {
                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    var plainText = Encoding.UTF8.GetBytes(s);
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(plainText, 0, plainText.Length));
                }
            }
        }
        private static string DecryptStringWeak(string be)
        {
            var key = Encoding.UTF8.GetBytes(InternalEncryptionKeys.EmailConfirmationKey);
            var iv = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var e = Convert.FromBase64String(be);

            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateDecryptor(key, iv))
            {
                var decryptedBytes = encryptor
                    .TransformFinalBlock(e, 0, e.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }


        public RUserIdDecryptionResult GetUserIdFromEmailConfirmationParameter(string parameter)
        {
            try
            {
                if (!Guid.TryParse(DecryptStringWeak(parameter), out var userId))
                {
                    _logging.LogWarning($"Passed parameter \"{parameter}\" is not convertable to GUID.");

                    return new RUserIdDecryptionResult
                    {
                        Status = "Passed parameter is not convertable to GUID.",
                        IsSuccessful = false
                    };
                }

                return new RUserIdDecryptionResult
                {
                    UserId = userId,
                    IsSuccessful = true,
                    Status = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return AServiceCallResult.BuildForException<RUserIdDecryptionResult>(ex);
            }
        }

        public async Task<RAccountCreationResult> ConfirmEmailAndCreateAccount(Guid userId)
        {
            try
            {
                using (var transaction = _GsContext.BeginTransaction())
                {
                    using (_ = _LsContext.BeginTransaction())
                    {
                        await _userRepository.ConfirmEmail(userId); // Writes to login server

                        var accountObject = new MAccount
                        {
                            DateCreated = DateTime.UtcNow,

                            IsPendingDeletion = false,
                            IsLocked = false,
                            IsSuspended = false,


                            UserId = userId,
                            LastLoginTime = DateTime.UtcNow,
                            // InventoryArray = "[]",
                        };

                        await _accountRepository.Create(accountObject); // Writes to game server

                        transaction.Complete();

                        return new RAccountCreationResult
                        {
                            Account = accountObject,
                            IsSuccessful = true,
                            Status = string.Empty
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<RAccountCreationResult>(ex);
            }
        }

        public async Task<RUserCreationResult> CreateUser(
            string username,
            string email,
            string password)
        {
            try
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt, false, HashType.SHA384);

                using (var transaction = _LsContext.BeginTransaction())
                {
                    var userObject = await _userRepository.Create(new MUser
                    {
                        DateCreated = DateTime.UtcNow,
                        IsPendingDeletion = false,
                        Email = email,
                        Username = username,
                        IsEmailConfirmed = false,
                        IsPhoneConfirmed = false,
                        Is2FAEnabled = false,
                        PasswordHash = passwordHash
                    });

                    transaction.Complete();

                    return new RUserCreationResult
                    {
                        User = userObject,
                        EmailConfirmationParameter = EncryptStringWeak(userObject.UserId.ToString()),

                        IsSuccessful = true,
                        Status = string.Empty
                    };

                }


            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<RUserCreationResult>(ex);
            }
        }




    }
}
