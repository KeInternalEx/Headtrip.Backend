using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Models.Instance;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly IContext<HeadtripGameServerContext> _context;
        public ZoneRepository(IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Zone>> GetAllZones()
        {
            return await _context.Connection.QueryAsync<Zone>(
                sql: "[gsZones_GetAllZones]",
                commandType: System.Data.CommandType.StoredProcedure);

        }


    }
}
