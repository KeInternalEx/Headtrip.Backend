using Headtrip.GameServerContext;
using Headtrip.Objects.Account;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.GameServer
{
    public sealed class AccountRepository : ASqlRepository<MAccount, HeadtripGameServerContext>, IAccountRepository
    {
        public AccountRepository(IContext<HeadtripGameServerContext> context) : base(context, "dbo.Accounts") { }

        #region IGenericRepository<MAccount, Guid>
        public async Task<MAccount> Create(MAccount Object)
            => await QuerySingleAsync<MAccount, MAccount>("Account_Create", Object);

        public async Task<MAccount> Read(Guid ObjectId)
            => await QuerySingleAsync<MAccount, Guid>("Account_Read", ObjectId);

        public async Task<IEnumerable<MAccount>> ReadAll()
            => await QueryAsync<MAccount>("Account_ReadAll");

        public async Task<MAccount> Update(MAccount Object)
            => await QuerySingleAsync<MAccount, MAccount>("Account_Update", Object);

        public async Task<MAccount> Delete(Guid ObjectId)
            => await QuerySingleAsync<MAccount, Guid>("Account_Delete", ObjectId);
        #endregion


        #region IAccountRepository

        public async Task<MAccount> ReadByUserId(Guid UserId)
            => await QuerySingleAsync<MAccount, Guid>("Account_ReadByUserId", UserId);

        #endregion


    }

}
