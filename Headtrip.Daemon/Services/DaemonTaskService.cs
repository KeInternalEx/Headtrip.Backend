using Headtrip.Daemon.Services.Abstract;
using Headtrip.Daemon.State.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Models;
using Headtrip.Models.Abstract;
using Headtrip.Models.Daemon;
using Headtrip.Models.Instance;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace Headtrip.Daemon.Services
{
    public class DaemonTaskService : IDaemonTaskService
    {

        public static readonly float SERVER_LATENCY_WORST_QUALITY = 99999;
        public static readonly float SERVER_LATENCY_BAD_QUALITY = 100;
        public static readonly float SERVER_LATENCY_AVG_QUALITY = 80;
        public static readonly float SERVER_LATENCY_HIGH_QUALITY = 60;

        public static readonly float[] SERVER_LATENCY_TIERS =
        {
            SERVER_LATENCY_HIGH_QUALITY,
            SERVER_LATENCY_AVG_QUALITY,
            SERVER_LATENCY_BAD_QUALITY,
            SERVER_LATENCY_WORST_QUALITY
        };




        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IDaemonState _daemonState;
        private readonly IDaemonRepository _daemonRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IZoneRepository _zoneRepository;

        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;

        public DaemonTaskService(
            ILogging<HeadtripGameServerContext> logging,
            IDaemonState daemonState,
            IDaemonRepository daemonRepository,
            IChannelRepository channelRepository,
            IZoneRepository zoneRepository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _logging = logging;
            _daemonState = daemonState;
            _daemonRepository = daemonRepository;
            _channelRepository = channelRepository;
            _zoneRepository = zoneRepository;
            _gsUnitOfWork = gsUnitOfWork;
        }

        /// <summary>
        /// Checks if the daemon state is initialized and creates a default instance of the passed AServiceCallResult inheriting type
        /// </summary>
        /// <typeparam name="T">The type of AServiceCallResult to create.</typeparam>
        /// <param name="result">Gets initialized to a default instance of the passed typeparam.
        /// If the method call returns false, the object is initialized with a status message explaining that the daemon state
        /// is uninitialized.</param>
        /// <returns>
        /// true if the daemon state has been initialized
        /// </returns>
        private bool CreateResultAndCheckDaemonState<T>(out T result) where T: AServiceCallResult, new()
        {
            if (!_daemonState.IsReady())
            {
                result = new T
                {
                    IsSuccessful = false,
                    Status = "Daemon State is uninitialized"
                };

                return false;
            }

            result = new T
            {
                IsSuccessful = false,
                Status = string.Empty
            };

            return true;
        }


        public async Task<GetDaemonContractGroupsResult> GetDaemonContractGroups()
        {

            if (!CreateResultAndCheckDaemonState(out GetDaemonContractGroupsResult result))
                return result;

            if (!_daemonState.IsSuperDaemon()!.Value)
            {
                result.IsSuccessful = false;
                result.Status = "IDaemonTaskService::GetDaemonContractGroups cannot be called from a non super daemon";

                return result;
            }

            result.ContractGroups = new List<DaemonContractGroup>();


            try
            {
                var daemons = await _daemonRepository.GetAllDaemons();
                if (daemons.Count() == 0) {
                    result.IsSuccessful = false;
                    result.Status = "There are no active daemons.";

                    return result;
                }


                var daemonContracts = await _daemonRepository.GetAllTransformableDaemonContracts();
                if (daemonContracts.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = "There are currently no transformable daemon contracts.";

                    return result;
                }

                var daemonLatencyRecords = await _daemonRepository.GetLatencyRecordsForTransformableContracts();
                if (daemonLatencyRecords.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = "There are transformable contracts, but no latency records describing their ping map.";

                    return result;
                }

                var zoneNames = daemonContracts.Select((c) => c.ZoneName!).Distinct();

                var contractGroupsByDaemonId = daemons.ToDictionary((daemon) => daemon.DaemonId, (daemon) => new List<DaemonContractGroup>());
                var daemonsByDaemonId = daemons.ToDictionary((daemon) => daemon.DaemonId, (daemon) => daemon);
                
                var daemonLatencyRecordsByAccountId = daemonLatencyRecords.ToDictionary((record) => record.AccountId, (record) => new List<DaemonLatencyRecord>());
                var daemonLatencyRecordsByDaemonId = daemonLatencyRecords.ToDictionary((record) => record.DaemonId, (record) => new List<DaemonLatencyRecord>());

                var relevantZonesByName = (await _zoneRepository.GetAllZones())
                    .Where((zone) => zoneNames.Contains(zone.ZoneName))
                    .ToDictionary((zone) => zone.ZoneName!, (zone) => zone);

                var contractsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName!, (zoneName) => new List<DaemonContract>());

                foreach (var contract in daemonContracts)
                    contractsByZoneName[contract.ZoneName].Add(contract);

                foreach (var record in daemonLatencyRecords)
                {
                    daemonLatencyRecordsByAccountId[record.AccountId].Add(record);
                    daemonLatencyRecordsByDaemonId[record.DaemonId].Add(record);
                }

                foreach (var zoneName in zoneNames)
                {
                    
                    var zone = relevantZonesByName[zoneName];
                    var contracts = contractsByZoneName[zoneName];

                    var contractsWithParties = contracts.Where((contract) => contract.PartyId.HasValue);
                    var contractsByPartyId = contractsWithParties.ToDictionary((contract) => contract.PartyId!.Value, (contract) => new List<DaemonContract>());

                    foreach (var partyContract in contractsWithParties)
                        contractsByPartyId[partyContract.PartyId!.Value].Add(partyContract);

                    var partyMappings = new Stack<DaemonPartyMapping>(
                        contractsByPartyId.Values
                            .Where((partyContracts) => partyContracts.Count > 1)
                            .Select((partyContracts) =>
                            {
                                var partyMapping = new DaemonPartyMapping
                                {
                                    ZoneName = zoneName,
                                    Contracts = partyContracts,
                                    AverageLatencyByDaemonId = new Dictionary<Guid, float>()
                                };

                                foreach (var daemon in daemons)
                                {
                                    var avgLatency = daemonLatencyRecords
                                        .Where((record) => partyContracts.Exists((ctr) => ctr.AccountId == record.AccountId))
                                        .Select((record) => record.Latency)
                                        .Average();

                                    partyMapping.AverageLatencyByDaemonId.Add(daemon.DaemonId, avgLatency);
                                }

                                return partyMapping;
                            })
                            .OrderByDescending((partyMapping) => partyMapping.Contracts.Count)
                            .ToArray());

                    // Assign parties first, creating groups as necessary

                    while (partyMappings.Count > 0)
                    {
                        var party = partyMappings.Pop();

                        foreach (var latencyLimit in SERVER_LATENCY_TIERS)
                        {
                            var possibleDaemonsForLatencyTier = daemons
                                .Where((daemon) => party.AverageLatencyByDaemonId[daemon.DaemonId] <= latencyLimit)
                                .OrderBy((daemon) => party.AverageLatencyByDaemonId[daemon.DaemonId]);

                            if (possibleDaemonsForLatencyTier.Count() == 0)
                                continue;

                            var possibleExistingGroupsForLatencyTier = contractGroupsByDaemonId
                                .Where((kv) =>
                                    possibleDaemonsForLatencyTier.Any((daemon) => daemon.DaemonId == kv.Key) &&
                                    kv.Value.Any((group) => 
                                        zone.ZoneName == zoneName &&
                                        zone.HardPlayerCap - group.NumberOfPlayers >= party.Contracts.Count));

                            // If there are no groups that will fit this party, we need to make a new one.
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var targetDaemon = possibleDaemonsForLatencyTier
                                    .FirstOrDefault((daemon) => daemon.NumberOfFreeEntries > 0);

                                // There are no daemons with space for a free entry, we'll have to go to a higher latency tier
                                if (targetDaemon == null)
                                    continue;

                                // Create the new contract group for the target daemon
                                var newContractGroup = new DaemonContractGroup
                                {
                                    Contracts = party.Contracts,
                                    DaemonId = targetDaemon.DaemonId,
                                    DaemonContractGroupId = Guid.NewGuid(),
                                    NumberOfParties = 1,
                                    NumberOfPlayers = (byte)party.Contracts.Count,
                                    ZoneName = zoneName
                                };

                                // Add the contract group to the list for this daemon
                                contractGroupsByDaemonId[targetDaemon.DaemonId].Add(newContractGroup);

                                // Remove the contracts referenced by this party so they don't get processed again later
                                contracts.RemoveAll((contract) => party.Contracts.Any((pc) => pc.DaemonContractId == contract.DaemonContractId));
                                
                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping and least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .OrderBy((kv) => party.AverageLatencyByDaemonId[kv.Key])
                                .Select((kv) => kv.Value.MinBy((group) => group.NumberOfPlayers))
                                .Where((group) => group.ZoneName == zoneName)
                                .First(); // This should never throw, if it does then something is seriously wrong.

                            group.NumberOfParties++;
                            group.NumberOfPlayers += (byte)party.Contracts.Count;
                            group.Contracts.AddRange(party.Contracts);

                            contracts.RemoveAll((contract) => party.Contracts.Any((pc) => pc.DaemonContractId == contract.DaemonContractId));

                            break;
                        }
                    }

                    // Parties have been assigned, assign individual contracts now
                    var remainingContracts = new Stack<DaemonContract>(contracts);

                    while (remainingContracts.Count > 0)
                    {
                        var currentContract = remainingContracts.Pop();

                        foreach (var latencyLimit in SERVER_LATENCY_TIERS)
                        {
                            var latencyRecordsForTier = daemonLatencyRecordsByAccountId[currentContract.AccountId]
                                .Where((record) => record.Latency <= latencyLimit)
                                .OrderBy((record) => record.Latency);


                            // There are no latency records that fit inside of this tier, move to a higher tier.
                            if (latencyRecordsForTier.Count() == 0)
                                continue;



                            var possibleExistingGroupsForLatencyTier = contractGroupsByDaemonId
                                .Where((kv) =>
                                    latencyRecordsForTier.Any((record) => record.DaemonId == kv.Key) &&
                                    kv.Value.Any((group) =>
                                        zone.ZoneName == zoneName &&
                                        zone.SoftPlayerCap - group.NumberOfPlayers > 0)); // Use the soft player cap to try to maintain server performance

                            // If there are no groups that will fit this party, we need to make a new one.
                            // We should also select a daemon that has the lowest possible ping for the remaining contracts
                            // Because it's very likely that we will be sending further contracts over to this group
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var possibleTargets = latencyRecordsForTier
                                    .Where((daemon) => daemonsByDaemonId[daemon.DaemonId].NumberOfFreeEntries > 0);

                                // There are no daemons with space for a free entry, we'll have to go to a higher latency tier
                                if (possibleTargets.Count() == 0)
                                    continue;



                                var bestPossibleTarget = possibleTargets
                                    .OrderBy((r) => remainingContracts
                                        .Select((contract) =>
                                            daemonLatencyRecordsByAccountId[contract.AccountId]
                                                .FirstOrDefault((record) => record.DaemonId == r.DaemonId))
                                        .Where((latencyRecord) =>
                                            latencyRecord != null &&
                                            latencyRecord.Latency <= latencyLimit)
                                        .Average((record) => record.Latency))
                                    .FirstOrDefault();

                                // Couldn't find a target daemon
                                if (bestPossibleTarget == null)
                                    continue;


                                // Create the new contract group for the target daemon
                                var newContractGroup = new DaemonContractGroup
                                {
                                    Contracts = new List<DaemonContract>() { currentContract },
                                    DaemonId = bestPossibleTarget.DaemonId,
                                    DaemonContractGroupId = Guid.NewGuid(),
                                    NumberOfParties = 0,
                                    NumberOfPlayers = 1,
                                    ZoneName = zoneName
                                };

                                // Add the contract group to the list for this daemon
                                contractGroupsByDaemonId[bestPossibleTarget.DaemonId].Add(newContractGroup);
                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping and least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .OrderBy((kv) => daemonLatencyRecordsByDaemonId[kv.Key].First((r) => r.AccountId == currentContract.AccountId).Latency)
                                .Select((kv) => kv.Value.MaxBy((group) => group.NumberOfPlayers)) 
                                .Where((group) => group.ZoneName == zoneName)
                                .First(); // This should never throw, if it does then something is seriously wrong.

                            group.NumberOfPlayers++;
                            group.Contracts.Add(currentContract);

                            break;
                        }
                    }
                }



               


                result.IsSuccessful = true;
                result.ContractGroups = contractGroupsByDaemonId.Values.SelectMany((x) => x).ToList();
                result.Status = $"Successfully created {result.ContractGroups.Count} contract groups.";



                return result;
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<GetDaemonContractGroupsResult>(ex); ;
            }
            finally
            {
                // _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


        public async Task<ServiceCallResult> ProcessDaemonContractGroups(List<DaemonContractGroup> daemonContractGroups)
        {
            var result = new ServiceCallResult { };

            try
            {
                _gsUnitOfWork.BeginTransaction();

                foreach (var contractGroup in daemonContractGroups)
                {
                    await _daemonRepository.ProcessDaemonContractGroup(
                        string.Join(",", contractGroup.Contracts.Select((contract) => contract.DaemonContractId.ToString("D"))),
                        contractGroup.DaemonId,
                        contractGroup.DaemonContractGroupId,
                        contractGroup.ZoneName);
                }


                result.IsSuccessful = true;
                result.Status = $"Successfully inserted {daemonContractGroups.Count} daemon contract groups";

                return result;
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<ServiceCallResult>(ex);
            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


        public async Task<ServiceCallResult> BeginProcessingTransformedDaemonContracts()
        {
            // TODO: NEED TO GROUP CONTRACTS ON THEIR CONTRACT GROUP ID FIELD
            // TODO: NEED TO MAKE A SERVER INSTANCE FOR EACH CONTRACT GROUP
            // TODO: NEED TO ASSIGN SERVER INSTANCE A CHANNEL ID AND PUT IT INTO THE POOL OF SERVERS
            // TODO: NEED TO UPDATE CONTRACT OBJECTS IN THE DATABASE WITH THE CHANNEL ID

            if (!CreateResultAndCheckDaemonState(out ServiceCallResult result))
                return result;

            var daemonId = _daemonState.GetDaemonId()!.Value;

            try
            {
                _gsUnitOfWork.BeginTransaction();

                var contracts = await _daemonRepository.GetAllTransformedDaemonContracts(daemonId);
                if (contracts.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = $"There are currently no transformed contracts for daemonId {daemonId}";

                    return result;
                }

                var contractsByGroupId = contracts.GroupBy((contract) => contract.DaemonContractId);
                
                // TODO: SPIN UP A SERVER FOR EACH GROUP
                // TODO: CREATE A CHANNEL IN THE DATABASE FOR EACH GROUP
                // TODO: UPDATE EACH GROUP WITH THE CHANNEL ID



            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<ServiceCallResult>(ex);

            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }



        }

        public async Task<ServiceCallResult> ProcessPendingDaemonContracts()
        {
            if (!CreateResultAndCheckDaemonState(out ServiceCallResult result))
                return result;

            var daemonId = _daemonState.GetDaemonId()!.Value;

            try
            {
                _gsUnitOfWork.BeginTransaction();


                var contractsToProcess = await _daemonRepository.BeginProcessingPendingContracts(daemonId);
                if (contractsToProcess.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = $"No contracts to process for daemon {daemonId}";

                    return result;
                }

                var channels = await _channelRepository.GetAllChannels();
                if (channels.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = $"There are no channels to load the contracts into!";

                    return result;
                }


                // TODO: QUEUE IPC SEND TO UNREAL SERVER INSTANCE
                //

                foreach (var contract in contractsToProcess)
                {
                    var serverInstance = _daemonState.ServersByChannelId[contract.TargetChannelId!.Value];
                    if (serverInstance == null)
                        throw new Exception($"Received contract on daemon {daemonId} with invalid channel id {contract.TargetChannelId.Value}");


                }




                await _daemonRepository.FinishProcessingPendingContracts(daemonId);

                result.IsSuccessful = true;
                result.Status = $"Successfully processed and removed {contractsToProcess.Count()} completed contracts from the database.";

                return result;
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<ServiceCallResult>(ex);
            }
            finally
            {
                _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }
    }
}
