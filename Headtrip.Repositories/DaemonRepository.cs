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
                sql: "[gsDaemons_GetOrCreateDaemonByNickname]",
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
                sql: "[gsDaemons_GetDaemonByDaemonId]",
                param: new
                {
                    DaemonId = daemonId
                },
                transaction: _context.Transaction,
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

        public async Task EndProcessingPendingContracts(Guid daemonId)
        {
            await _context.Connection.QueryAsync<DaemonContract>(
                sql: "[gsDaemonContracts_EndProcessingPendingContracts]",
                param: new
                {
                    DaemonId = daemonId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Daemon>> GetAllDaemons()
        {
            return await _context.Connection.QueryAsync<Daemon>(
                sql: "[gsDaemons_GetAllDaemons",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<DaemonContract>> GetAllTransformableDaemonContracts()
        {
            return await _context.Connection.QueryAsync<DaemonContract>(
                sql: "[gsDaemonContracts_GetAllTransformableDaemonContracts]",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task ProcessDaemonContractGroup(string daemonContractIds, Guid daemonId, Guid daemonContractGroupId, string zoneName)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[gsDaemonContracts_ProcessDaemonContractGroup]",
                param: new
                {
                    DaemonContractIds = daemonContractIds,
                    DaemonId = daemonId,
                    DaemonContractGroupId = daemonContractGroupId,
                    ZoneName = zoneName
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<DaemonLatencyRecord>> GetLatencyRecordsForTransformableContracts()
        {
            return await _context.Connection.QueryAsync<DaemonLatencyRecord>(
                sql: "[gsDaemonLatencyRecords_GetLatencyRecordsForTransformableContracts]",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
