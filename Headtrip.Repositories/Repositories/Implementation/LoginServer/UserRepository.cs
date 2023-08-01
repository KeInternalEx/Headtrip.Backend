using Dapper;
using Headtrip.LoginServerContext;
using Headtrip.Objects.User;
using Headtrip.Repositories.Repositories.Interface.LoginServer;
using Headtrip.Repositories.Sql;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Implementation.LoginServer
{
    public class UserRepository : ASqlRepository<MUser, HeadtripLoginServerContext>, IUserRepository
    {
        public UserRepository(IContext<HeadtripLoginServerContext> context) : base(context, "dbo.Users") { }


        #region IGenericRepository<MUser, Guid>
        public async Task<MUser> Create(MUser Object)
            => await QuerySingleAsync<MUser, MUser>("User_Create", Object);

        public async Task<MUser> Read(Guid ObjectId)
            => await QuerySingleAsync<MUser, Guid>("User_Read", ObjectId);

        public async Task<IEnumerable<MUser>> ReadAll()
            => await QueryAsync<MUser>("User_ReadAll");

        public async Task<MUser> Update(MUser Object)
            => await QuerySingleAsync<MUser, MUser>("User_Update", Object);

        public async Task<MUser> Delete(Guid ObjectId)
            => await QuerySingleAsync<MUser, Guid>("User_Delete", ObjectId);

        #endregion


        #region IUserRepository
        public async Task<MUser> ReadByUsername(string Username)
            => await QuerySingleAsync<MUser, string>("User_ReadByUsername", Username);

        public async Task<MUser> ReadByEmail(string Email)
            => await QuerySingleAsync<MUser, string>("User_ReadByEmail", Email);

        public async Task<MUser> ConfirmEmail(Guid UserId)
            => await QuerySingleAsync<MUser, Guid>("User_ConfirmEmail", UserId);

        #endregion



    }
}
