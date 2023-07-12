using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Models.Account;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IContext<HeadtripGameServerContext> _Context;

        public AccountRepository(IContext<HeadtripGameServerContext> context)
        {
            _Context = context;
        }


        public async Task CreateAccount(Account account)
        {
            var entriesInserted = await _Context.Connection.ExecuteAsync(
                sql: "[Account_CreateAccount]",
                param: account,
                transaction: _Context.Transaction,
                commandType: CommandType.StoredProcedure);

            if (entriesInserted <= 0)
                throw new Exception("Account creation failed, no entries inserted");
        }

    }

}
