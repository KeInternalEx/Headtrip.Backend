using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class UeServiceRepository : IUeServiceRepository
    {
        private IContext<HeadtripGameServerContext> _context;

        public UeServiceRepository(
            IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }

        public async Task<MUeService> GetOrCreateUeServiceByNickname(string nickname)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<MUeService>(
                sql: "[gsUeServices_GetOrCreateUeServiceByNickname]",
                param: new
                {
                    Nickname = nickname
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<MUeService> GetUeServiceByUeServiceId(Guid UeServiceId)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<MUeService>(
                sql: "[gsUeServices_GetUeServiceByUeServiceId]",
                param: new
                {
                    UeServiceId = UeServiceId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MUeServiceServerTransferRequest>> BeginProcessingPendingServerTransferRequests(Guid UeServiceId)
        {
            return await _context.Connection.QueryAsync<MUeServiceServerTransferRequest>(
                sql: "[gsUeServiceProc_BeginProcessingPendingServerTransferRequests]",
                param: new
                {
                    UeServiceId = UeServiceId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task FinishProcessingPendingServerTransferRequests(Guid UeServiceId)
        {
            await _context.Connection.QueryAsync<MUeServiceServerTransferRequest>(
                sql: "[gsUeServiceProc_FinishProcessingPendingServerTransferRequests]",
                param: new
                {
                    UeServiceId = UeServiceId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MUeService>> GetAllUeServices()
        {
            return await _context.Connection.QueryAsync<MUeService>(
                sql: "[gsUeServices_GetAllUeServices",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<MUeServiceServerTransferRequest>> GetAllTransformableUeServiceServerTransferRequests()
        {
            return await _context.Connection.QueryAsync<MUeServiceServerTransferRequest>(
                sql: "[gsUeServiceServerTransferRequests_GetAllTransformableUeServiceServerTransferRequests]",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MUeServiceServerTransferRequest>> GetAllTransformedUeServiceServerTransferRequests(Guid UeServiceId)
        {
            return await _context.Connection.QueryAsync<MUeServiceServerTransferRequest>(
                sql: "[gsUeServiceServerTransferRequests_GetAllTransformedUeServiceServerTransferRequests]",
                transaction: _context.Transaction,
                param: new
                {
                    UeServiceId = UeServiceId
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task ProcessUeServiceServerTransferRequestGroup(string UeServiceServerTransferRequestIds, Guid UeServiceId, Guid UeServiceServerTransferRequestGroupId, string zoneName)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[gsUeServiceServerTransferRequests_ProcessUeServiceServerTransferRequestGroup]",
                param: new
                {
                    UeServiceServerTransferRequestIds = UeServiceServerTransferRequestIds,
                    UeServiceId = UeServiceId,
                    UeServiceServerTransferRequestGroupId = UeServiceServerTransferRequestGroupId,
                    ZoneName = zoneName
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<mUeServiceLatencyRecord>> GetLatencyRecordsForTransformableServerTransferRequests()
        {
            return await _context.Connection.QueryAsync<mUeServiceLatencyRecord>(
                sql: "[gsUeServiceLatencyRecords_GetLatencyRecordsForTransformableServerTransferRequests]",
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
