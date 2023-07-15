using BCrypt.Net;
using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Models.Abstract;
using Headtrip.Models.Account;
using Headtrip.Models.User;
using Headtrip.Repositories.Abstract;
using Headtrip.Secrets;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
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

    public class UserService : IUserService
    {
        private readonly ILogging<HeadtripLoginServerContext> _logging;

        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;
        private readonly IUnitOfWork<HeadtripLoginServerContext> _lsUnitOfWork;
        private readonly IUnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext> _combinedUnitOfWork;


        public UserService(
            ILogging<HeadtripLoginServerContext> logging,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork,
            IUnitOfWork<HeadtripLoginServerContext> lsUnitOfWork,
            IUnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext> combinedUnitOfWork)
        {
            _logging = logging;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _gsUnitOfWork = gsUnitOfWork;
            _lsUnitOfWork = lsUnitOfWork;
            _combinedUnitOfWork = combinedUnitOfWork;
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


        public UserIdDecryptionResult GetUserIdFromEmailConfirmationParameter(string parameter)
        {
            try
            {
                if (!Guid.TryParse(DecryptStringWeak(parameter), out var userId))
                {
                    _logging.LogWarning($"Passed parameter \"{parameter}\" is not convertable to GUID.");

                    return new UserIdDecryptionResult
                    {
                        Status = "Passed parameter is not convertable to GUID.",
                        IsSuccessful = false
                    };
                }

                return new UserIdDecryptionResult
                {
                    UserId = userId,
                    IsSuccessful = true,
                    Status = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                return ServiceCallResult.BuildForException<UserIdDecryptionResult>(ex);
            }
        }

        public async Task<AccountCreationResult> ConfirmEmailAndCreateAccount(Guid userId)
        {
            try
            {
                _combinedUnitOfWork.BeginTransaction();
                await _userRepository.ConfirmEmail(userId); // Writes to login server

                var accountObject = new Account
                {
                    LastModifiedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,

                    IsMarkedForDeletion = false,
                    IsLocked = false,
                    IsSuspended = false,


                    UserId = userId,
                    LastLoginTime = DateTime.UtcNow,
                   // InventoryArray = "[]",
                    Money = 0,
                    TotalPlaytimeMs = 0,
                };

                await _accountRepository.CreateAccount(accountObject); // Writes to game server
                _combinedUnitOfWork.CommitTransaction();


                return new AccountCreationResult
                {
                    Account = accountObject,
                    IsSuccessful = true,
                    Status = string.Empty
                };
            }
            catch (Exception ex)
            {
                _combinedUnitOfWork.RollbackTransaction();
                _logging.LogException(ex);

                return ServiceCallResult.BuildForException<AccountCreationResult>(ex);
            }
        }

        public async Task<UserCreationResult> CreateUser(
            string username,
            string email,
            string password)
        {
            try
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt, false, HashType.SHA384);

                var userObject = new User
                {
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    Email = email,
                    Username = username,
                    IsEmailConfirmed = false,
                    IsPhoneConfirmed = false,
                    Is2FAEnabled = false,
                    PasswordHash = passwordHash,
                };

                _lsUnitOfWork.BeginTransaction();

                await _userRepository.CreateUser(userObject);

                _lsUnitOfWork.CommitTransaction();

                return new UserCreationResult
                {
                    User = userObject,
                    EmailConfirmationParameter = EncryptStringWeak(userObject.UserId.ToString()),

                    IsSuccessful = true,
                    Status = string.Empty
                };

            }
            catch (Exception ex)
            {
                _lsUnitOfWork.RollbackTransaction();
                _logging.LogException(ex);

                return ServiceCallResult.BuildForException<UserCreationResult>(ex);
            }
        }




    }
}
