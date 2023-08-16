using Headtrip.Objects.Account;
using Headtrip.Repositories.Generic;

namespace Headtrip.Repositories.Repositories.Interface
{
    public interface IAccountRepository : IGenericRepository<MAccount, Guid>
    {
        Task<MAccount> ReadByUserId(Guid UserId);


    }
}
