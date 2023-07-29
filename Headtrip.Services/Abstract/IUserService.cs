using Headtrip.Objects.Account;
using Headtrip.Objects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services.Abstract
{
    public interface IUserService
    {


        UserIdDecryptionResult GetUserIdFromEmailConfirmationParameter(string parameter);



        Task<UserCreationResult> CreateUser(string username, string email, string password);
        Task<AccountCreationResult> ConfirmEmailAndCreateAccount(Guid userId);



    }
}
