using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IContext _Context;

        public AccountRepository(IContext context)
        {
            _Context = context;
        }




    }

}
