using Headtrip.Objects.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IAccountRepository
    {

        Task CreateAccount(Account account);


        Task<Account> GetAccountByUserId(Guid userId);


    }
}
