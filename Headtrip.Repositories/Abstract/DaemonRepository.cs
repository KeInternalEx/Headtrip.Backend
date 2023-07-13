using Dapper;
using Headtrip.GameServerContext;
using Headtrip.Models.Daemon;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public class DaemonRepository : IDaemonRepository
    {
        private IContext<HeadtripGameServerContext> _context;

        public DaemonRepository(
            IContext<HeadtripGameServerContext> context)
        {
            _context = context;
        }

        public async Task CreateDaemon(Daemon daemon)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[Daemon_CreateDaemon]",
                param: daemon,
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Daemon> GetDaemonByDaemonId(Guid daemonId)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<Daemon>(
                sql: "[Daemon_GetDaemonByDaemonId]",
                param: new
                {
                    DaemonId = daemonId
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Daemon> GetDaemonByNickname(string nickname)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<Daemon>(
                sql: "[Daemon_GetDaemonByNickname]",
                param: new
                {
                    Nickname = nickname
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Daemon> GetOrCreateDaemon(string nickname)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<Daemon>(
                sql: "[Daemon_GetOrCreateDaemon]",
                param: new
                {
                    Nickname = nickname
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DaemonContract>> BeginProcessingPendingContracts(Guid daemonId)
        {
            return await _context.Connection.QueryAsync<DaemonContract>(
                sql: "[Daemon_BeginProcessingPendingContracts]",
                param: new
                {
                    DaemonId = daemonId
                },
                transaction: _context.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);

            /*
             * Update the subset of contracts that contain a CurrentChannelId that we manage
             * We only want the ones that have a valid TargetChannelId, the ones that don't need to be transformed first
             * We only want the contracts that we aren't currently processing.
             * We mark them as processing in case of an unexpected termination of the daemon.
             * We also use the IsProcessing flag as a discrimnator so that later
             * When we update the contracts that are in the IsProcessing state, we don't mark unprocessed contracts as fulfilled
             * 
             * 
             * 
             * UPDATE Contracts AS Contract SET
             *  Contract.IsProcessing = 1
             * WHERE
             *  Contract.CurrentDaemonId = @DaemonId AND
             *  Contract.TargetDaemonId IS NOT NULL AND
             *  Contract.IsProcessing = 0 AND
             *  Contract.IsTransforming = 0
             *  
             * SELECT * FROM Contracts WHERE
             *  Contract.IsProcessing = 1 AND
             *  Contract.IsTransforming = 0 AND
             *  Contract.CurrentDaemonId = @DaemonId;
             */
        }
        
        public async Task<IEnumerable<DaemonContract>> BeginTransformingPendingContracts(Guid daemonId, int numberOfFreeInstances)
        {
            return await _context.Connection.QueryAsync<DaemonContract>(
               sql: "[Daemon_BeginTransformingPendingContracts]",
               param: new
               {
                   DaemonId = daemonId,
                   FreeInstances = numberOfFreeInstances
               },
               transaction: _context.Transaction,
               commandType: System.Data.CommandType.StoredProcedure);

            /*
             * 
             * 
             * 
             * 
             * SELECT Contract.ZoneName AS ZoneLabel, COUNT(Contracts.ContractId) AS NumberOfContracts FROM Contracts AS Contract
             * WHERE
             *  Contract.IsTransforming = 0 AND
             *  Contract.TargetDaemonId IS NULL
             * GROUP BY Contract.ZoneName
             * ORDER BY NumberOfContracts DESC;
             * 
             * GIVES US ROWS:
             *  ZoneName, NumberOfContractsForThisZone
             *  
             * NUMBER OF CONTRACTS TO TAKE = SpacePresent * PlayerCap
             * NUMBER OF CLAIMS TO CHECK FOR = NumberOfContractsForThisZone / PlayerCap
             * 
             * 
             * 
             * -- BELOW INSERTS A CLAIM FOR EACH ZONE THAT DOESN'T HAVE ENOUGH CLAIMS TO SATISFY ALL OF THE CONTRACTS
             * INSERT INTO DaemonClaims
             * SELECT ZoneName, @DaemonId AS DaemonId FROM (
             * 
             * -- NEED TO MANIPULATE INTO A STATEMENT THAT INSERTS AS MANY CLAIMS PER ZONE AS WE HAVE SPACE FOR
             * 
             * 
             * 
             * SELECT
             *      ZoneName,
             *      @DaemonId AS DaemonId,
             *      CEIL(CAST(Aggregate.NumberOfContracts AS Float) / CAST(Aggregate.SoftPlayerCap AS Float)) AS NumberOfClaimsRequired
             * FROM (
             *      SELECT
             *          Contract.ZoneName,
             *          COUNT(Contract.ContractId) AS NumberOfContracts,
             *          COUNT(Claim.ZoneName) AS NumberOfClaims,
             *          Zone.SoftPlayerCap AS SoftPlayerCap
             *      FROM DaemonContract AS Contract
             *      INNER JOIN DaemonClaims AS Claim ON Claim.ZoneName = Contract.ZoneName
             *      INNER JOIN Zones AS Zone ON Zone.ZoneName = Contract.ZoneName
             *      WHERE
             *          Contract.IsTransforming = 0 AND
             *          Contract.TargetDaemonId IS NULL
             *      GROUP BY Contract.ZoneName
             *      ORDER BY NumberOfContracts DESC) AS Aggregate
             *  WHERE
             *      CAST(Aggregate.NumberOfContracts AS Float) > CAST(Aggregate.SoftPlayerCap AS Float) * CAST(Aggregate.NumberOfClaims AS Float)
             * 
             */
        }
    }
}
