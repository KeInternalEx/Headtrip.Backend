using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Models.Daemon;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class DaemonRepository : IDaemonRepository
    {
        private IContext<HeadtripGameServerContext> _context;

        public DaemonRepository(
            IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }

        public async Task<Daemon> GetOrCreateDaemonByNickname(string nickname)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<Daemon>(
                sql: "[gsDaemon_GetOrCreateDaemonByNickname]",
                param: new
                {
                    Nickname = nickname
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Daemon> GetDaemonByDaemonId(Guid daemonId)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<Daemon>(
                sql: "[gsDaemon_GetDaemonByDaemonId]",
                param: new
                {
                    DaemonId = daemonId
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DaemonContract>> BeginProcessingPendingContracts(Guid daemonId)
        {
            return await _context.Connection.QueryAsync<DaemonContract>(
                sql: "[gsDaemonContracts_BeginProcessingPendingContracts]",
                param: new
                {
                    DaemonId = daemonId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DaemonClaim>> CreateClaimsForTransformableContracts(Guid daemonId, int numberOfCreatableInstances)
        {
            return await _context.Connection.QueryAsync<DaemonClaim>(
               sql: "[gsDaemonClaims_CreateClaimsForTransformableContracts]",
               param: new
               {
                   DaemonId = daemonId,
                   FreeInstances = numberOfCreatableInstances
               },
               transaction: _context.Transaction,
               commandType: System.Data.CommandType.StoredProcedure);


        }

        public async Task<IEnumerable<DaemonContract>> GetAllTransformableDaemonContracts()
        {
            return await _context.Connection.QueryAsync<DaemonContract>(
                sql: "[gsDaemonContracts_GetAllTransformableDaemonContracts]",
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DaemonClaim>> GetAllDaemonClaims()
        {
            return await _context.Connection.QueryAsync<DaemonClaim>(
                sql: "[gsDaemonClaims_GetAllDaemonClaims]",
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
