using Headtrip.Objects.User;
using Headtrip.Repositories.Generic;

namespace Headtrip.Repositories.Repositories.Interface.LoginServer
{
    public interface IUserRepository : IGenericRepository<MUser, Guid>
    {
        Task<MUser> ReadByUsername(string Username);
        Task<MUser> ReadByEmail(string Email);
        Task<MUser> ConfirmEmail(Guid UserId);
    }
}
