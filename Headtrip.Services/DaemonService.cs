using Headtrip.GameServerContext;
using Headtrip.Models.Abstract;
using Headtrip.Models.Daemon;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services
{
    public class DaemonService : IDaemonService
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IDaemonRepository _daemonRepository;
        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;

        public DaemonService(
            ILogging<HeadtripGameServerContext> logging,
            IDaemonRepository daemonRepository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _logging = logging;
            _daemonRepository = daemonRepository;
            _gsUnitOfWork = gsUnitOfWork;
        }






        public async Task<CreateDaemonClaimsResult> CreateDaemonClaims(Guid daemonId, int freeServerSlots)
        {
            var result = new CreateDaemonClaimsResult
            {
                IsSuccessful = false,
                Status = string.Empty,
                Claims = null
            };

            try
            {
                _gsUnitOfWork.BeginTransaction();

                var daemonClaims = await _daemonRepository.CreateClaimsForTransformableContracts(daemonId, freeServerSlots);
                if (daemonClaims == null || daemonClaims.Count() == 0)
                {
                    result.Status = "No pending contracts";
                    result.IsSuccessful = true;

                    return result;
                }

                result.Claims = daemonClaims.ToList();
                result.IsSuccessful = true;

                return result;
            }
            catch (Exception ex)
            {
                result = ServiceCallResult.BuildForException<CreateDaemonClaimsResult>(ex);

                _logging.LogException(ex);

                return result;
            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


        public async Task<GetDaemonContractGroupsResult> GetDaemonContractGroups()
        {
            var result = new GetDaemonContractGroupsResult {
                IsSuccessful = false,
                Status = string.Empty,
                ContractGroups = null
            };

            try
            {



                var daemonClaims = await _daemonRepository.GetAllDaemonClaims();
                var daemonContracts = await _daemonRepository.GetAllTransformableDaemonContracts();


                var zoneNames = daemonClaims.Select((c) => c.ZoneName).Distinct();
                var contractGroupsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName ?? "", (zoneName) => new List<DaemonContractGroup>());
                var contractsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName ?? "", (zoneName) => new List<DaemonContract>());
                var partyMappingsByZone = zoneNames.ToDictionary((zoneName) => zoneName ?? "", (zoneName) => new List<DaemonPartyMapping>());

                foreach (var claim in daemonClaims)
                {
                    contractGroupsByZoneName[claim.ZoneName ?? ""].Add(new DaemonContractGroup
                    {
                        Claim = claim,
                        Contracts = new List<DaemonContract>(),
                        NumberOfParties = 0,
                        NumberOfPlayers = 0
                    });
                }

                foreach (var contract in daemonContracts)
                    contractsByZoneName[contract.ZoneName ?? ""].Add(contract);


                foreach (var zoneName in zoneNames)
                {
                    var contracts = contractsByZoneName[zoneName ?? ""];
                    var partyMapping = partyMappingsByZone[zoneName ?? ""];
                    var contractsWithParties = contracts.Where((contract) => contract.PartyId != null);
                    var contractsByPartyId = new Dictionary<Guid, List<DaemonContract>>();



                    
                }


                // TODO: GET ALL CLAIMS
                // TODO: GROUP CLAIMS BY ZONE NAME
                // TODO: FOR EACH GROUP, GET ALL CONTRACTS THAT NEED TO BE TRANSFORMED

                // TODO: IN THOSE CONTRACTS, IDENTIFY ALL OF THE CURRENT PARTIES (PLAYERS WHO HAVE THE SAME PARTY ID)
                // TODO: SORT THOSE PARTIES BY SIZE DESCENDING
                // TODO: ASSIGN THE PARTIES TO EACH SERVER
                // TODO: ASSIGN THE REST OF THE CONTRACTS (NON PARTY PLAYERS) BUT IN REVERSE (FILL LESS FULL SERVERS WITH SOLOS)

                // TODO: FOR EACH CONTRACT GROUP, SEND THE LIST OF ContractIds TO THE UPDATE SP, ALONG WITH THE DaemonId AND ChannelId, AND ZoneName
                // TODO: THAT SP WILL SPLIT THE SP'S PARAM INTO UNQIUE IDENTIFIERS, UPDATE ALL OF THE RECORDS WITH THE DAEMON ID (TransformingDaemonId)
                // TODO: AND THEN FINALLY DELETE THE CLAIM


            }
            catch (Exception ex)
            {
                result = ServiceCallResult.BuildForException<GetDaemonContractGroupsResult>(ex);

                _logging.LogException(ex);

                return result;
            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


    }
}
