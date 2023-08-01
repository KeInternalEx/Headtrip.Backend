using Headtrip.Objects.UeService;
using Headtrip.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Repositories.Interface.GameServer
{
    public interface IUeServiceRepository : IGenericRepository<MUeService, Guid>
    {
        Task<MUeService> ReadByNickname(string nickname);



    }
}
