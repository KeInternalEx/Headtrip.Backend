using Headtrip.GameServerContext;
using Headtrip.Objects.Account;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;
        
        public AccountService(
            IAccountRepository accountRepository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _accountRepository = accountRepository;
            _gsUnitOfWork = gsUnitOfWork;
        }


     



    }
}
