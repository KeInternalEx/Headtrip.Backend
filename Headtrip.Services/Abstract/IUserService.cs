using Headtrip.Objects.Account.Result;
using Headtrip.Objects.User.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services.Abstract
{
    public interface IUserService
    {


        RUserIdDecryptionResult GetUserIdFromEmailConfirmationParameter(string parameter);



        Task<RUserCreationResult> CreateUser(string username, string email, string password);
        Task<RAccountCreationResult> ConfirmEmailAndCreateAccount(Guid userId);



    }
}
