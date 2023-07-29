using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.Account;
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
            await _Context.Connection.ExecuteAsync(
                sql: "[Account_CreateAccount]",
                param: account,
                transaction: _Context.Transaction,
                commandType: CommandType.StoredProcedure);

        }

        public async Task<Account> GetAccountByUserId(Guid userId)
        {
            return await _Context.Connection.QueryFirstOrDefaultAsync<Account>(
                sql: "[Account_GetAccountByUserId]",
                param: userId,
                commandType: CommandType.StoredProcedure);
        }
    }

}
