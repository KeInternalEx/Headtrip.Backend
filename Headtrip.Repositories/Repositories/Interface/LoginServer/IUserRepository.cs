using Headtrip.Objects.User;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface.LoginServer
{
    public interface IUserRepository : IGenericRepository<MUser, Guid>
    {
        Task<MUser> ReadByUsername(string username);
        Task<MUser> ReadByEmail(string email);
        Task<MUser> ConfirmEmail(Guid userId);
    }
}
