using Headtrip.GameServerContext;
using Headtrip.Objects.Account;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly IAccountRepository _AccountRepository;
        private readonly IContext<HeadtripGameServerContext> _Context;
        
        public AccountService(
            IAccountRepository AccountRepository,
            IContext<HeadtripGameServerContext> Context)
        {
            _AccountRepository = AccountRepository;
            _Context = Context;
        }


     



    }
}
