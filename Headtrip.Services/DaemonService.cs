using Headtrip.GameServerContext;
using Headtrip.Models.Abstract;
using Headtrip.Models.Daemon;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;

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

        public async Task<GetDaemonContractGroupsResult> GetDaemonContractGroups()
        {
            var result = new GetDaemonContractGroupsResult {
                IsSuccessful = false,
                Status = string.Empty,
                ContractGroups = new List<DaemonContractGroup>()
            };

            try
            {

                var daemonClaims = await _daemonRepository.GetAllDaemonClaims();
                var daemonContracts = await _daemonRepository.GetAllTransformableDaemonContracts();


                var zoneNames = daemonClaims.Select((c) => c.ZoneName!).Distinct();
                var contractGroupsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName!, (zoneName) => new List<DaemonContractGroup>());
                var contractsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName!, (zoneName) => new List<DaemonContract>());


                foreach (var claim in daemonClaims)
                {
                    contractGroupsByZoneName[claim.ZoneName].Add(new DaemonContractGroup
                    {
                        Claim = claim,
                        Contracts = new List<DaemonContract>(),
                        NumberOfParties = 0,
                        NumberOfPlayers = 0
                    });
                }

                foreach (var contract in daemonContracts)
                    contractsByZoneName[contract.ZoneName].Add(contract);


                foreach (var zoneName in zoneNames)
                {
                    var contractGroups = contractGroupsByZoneName[zoneName];
                    var contracts = contractsByZoneName[zoneName];
                    var contractsWithParties = contracts.Where((contract) => contract.PartyId.HasValue);

                    // Continue on if we don't have any parties in this zone
                    if (contractsWithParties.Count() == 0)
                        continue;

                    var contractsByPartyId = contractsWithParties.ToDictionary((contract) => contract.PartyId!.Value, (contract) => new List<DaemonContract>());


                    foreach (var partyContract in contractsWithParties)
                        contractsByPartyId[partyContract.PartyId!.Value].Add(partyContract);


                    // Calling pop on this stack gets us descending order
                    // We only care about the parties that have more than one person in them
                    var partyMappings = new Stack<DaemonPartyMapping>(
                        contractsByPartyId.Values.Where((party) => party.Count > 1).OrderBy((party) => party.Count).Select((party) =>
                        {
                            return new DaemonPartyMapping
                            {
                                ZoneName = zoneName,
                                Contracts = party
                            };
                        }).ToArray());


                    int currentContractGroupIndex = 0;
                    var currentContractGroup = contractGroups[currentContractGroupIndex];

                    while (
                        partyMappings.Count > 0 &&
                        currentContractGroupIndex < contractGroups.Count)
                    {

                        var currentParty = partyMappings.Pop();

                        // If the party doesn't fit into this contract group, advance to the next group.
                        if (currentParty.Contracts.Count > currentContractGroup.Claim.NumberOfContracts - currentContractGroup.NumberOfPlayers)
                        {
                            currentContractGroupIndex++;
                            currentContractGroup = contractGroups[currentContractGroupIndex];
                        }

                        // Update the target daemon id for these contracts
                        foreach (var partyContract in currentParty.Contracts)
                            partyContract.TargetDaemonId = currentContractGroup.Claim.DaemonId;

                        currentContractGroup.NumberOfParties++;
                        currentContractGroup.NumberOfPlayers += (byte)currentParty.Contracts.Count;
                        currentContractGroup.Contracts.AddRange(currentParty.Contracts);

                        contracts.RemoveAll(currentParty.Contracts.Contains); // Remove these contracts from the zone's contract pool
                    }

                    currentContractGroupIndex = contractGroups.Count - 1;
                    currentContractGroup = contractGroups[currentContractGroupIndex];


                    foreach (var contract in contracts)
                    {
                        // Make sure current contract group has space to add more contracts, if it doesn't then back it up
                        while (
                            currentContractGroup.NumberOfPlayers == currentContractGroup.Claim.NumberOfContracts &&
                            currentContractGroupIndex > 0)
                        {
                            currentContractGroupIndex--;
                            currentContractGroup = contractGroups[currentContractGroupIndex];
                        }

                        if (currentContractGroup.NumberOfPlayers == currentContractGroup.Claim.NumberOfContracts &&
                            currentContractGroupIndex == 0)
                        {
                            break;
                        }

                        contract.TargetDaemonId = currentContractGroup.Claim.DaemonId;


                        currentContractGroup.NumberOfPlayers++;
                        currentContractGroup.Contracts.Add(contract);
                    }


                    result.ContractGroups.AddRange(contractGroups);
                }



                result.IsSuccessful = true;
                result.Status = $"Successfully created {result.ContractGroups.Count} contract groups.";



                return result;
            }
            catch (Exception ex)
            {
                result = ServiceCallResult.BuildForException<GetDaemonContractGroupsResult>(ex);

                _logging.LogException(ex);

                return result;
            }
            finally
            {
                // _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


        public async Task<ServiceCallResult> ProcessDaemonContractGroups(List<DaemonContractGroup> daemonContractGroups)
        {
            var result = new ServiceCallResult
            {
                IsSuccessful = false,
                Status = string.Empty
            };

            try
            {
                _gsUnitOfWork.BeginTransaction();

                foreach (var contractGroup in daemonContractGroups)
                {
                    await _daemonRepository.ProcessDaemonContractGroup(
                        string.Join(",", contractGroup.Contracts.Select((contract) => contract.DaemonContractId.ToString("D"))),
                        contractGroup.Claim.DaemonId,
                        contractGroup.Claim.ZoneName);
                }

                _gsUnitOfWork.CommitTransaction();

                result.IsSuccessful = true;
                result.Status = $"Successfully inserted {daemonContractGroups.Count} daemon contract groups";

                return result;
            }
            catch (Exception ex)
            {
                _gsUnitOfWork.RollbackTransaction();
                _logging.LogException(ex);

                result = ServiceCallResult.BuildForException<ServiceCallResult>(ex);

                return result;
            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


    }
}
