using Headtrip.Objects.Character;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface
{
    public interface ICharacterRepository : IGenericRepository<MCharacter, Guid>
    {
        /*
         *     public interface IAccountRepository : IGenericRepository<MAccount, Guid>
    {
        Task<MAccount> ReadByUserId(Guid UserId);


    }
         */
    }
}
