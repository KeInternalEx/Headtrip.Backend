using Headtrip.UeService.Services.Abstract;
using Headtrip.UeService.State.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Objects;
using Headtrip.Objects.UeService;
using Headtrip.Objects.Instance;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using Headtrip.Objects.Abstract.Results;

namespace Headtrip.UeService.Services
{
    public sealed class UeServiceTaskService : IUeServiceTaskService
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
        private readonly IUeServiceState _UeServiceState;
        private readonly IUeServiceRepository _UeServiceRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IZoneRepository _zoneRepository;

        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;

        public UeServiceTaskService(
            ILogging<HeadtripGameServerContext> logging,
            IUeServiceState UeServiceState,
            IUeServiceRepository UeServiceRepository,
            IChannelRepository channelRepository,
            IZoneRepository zoneRepository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _logging = logging;
            _UeServiceState = UeServiceState;
            _UeServiceRepository = UeServiceRepository;
            _channelRepository = channelRepository;
            _zoneRepository = zoneRepository;
            _gsUnitOfWork = gsUnitOfWork;
        }

        /// <summary>
        /// Checks if the UeService state is initialized and creates a default instance of the passed AServiceCallResult inheriting type
        /// </summary>
        /// <typeparam name="T">The type of AServiceCallResult to create.</typeparam>
        /// <param name="result">Gets initialized to a default instance of the passed typeparam.
        /// If the method call returns false, the object is initialized with a status message explaining that the UeService state
        /// is uninitialized.</param>
        /// <returns>
        /// true if the UeService state has been initialized
        /// </returns>
        private bool CreateResultAndCheckUeServiceState<T>(out T result) where T: AServiceCallResult, new()
        {
            if (!_UeServiceState.IsReady())
            {
                result = new T
                {
                    IsSuccessful = false,
                    Status = "UeService State is uninitialized"
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


        public async Task<RGetUeServiceServerTransferRequestGroupsResult> GetUeServiceServerTransferRequestGroups()
        {

            if (!CreateResultAndCheckUeServiceState(out RGetUeServiceServerTransferRequestGroupsResult result))
                return result;

            if (!_UeServiceState.IsSuperUeService()!.Value)
            {
                result.IsSuccessful = false;
                result.Status = "IUeServiceTaskService::GetUeServiceServerTransferRequestGroups cannot be called from a non super UeService";

                return result;
            }

            result.ServerTransferRequestGroups = new List<UeServiceServerTransferRequestGroup>();


            try
            {
                var UeServices = await _UeServiceRepository.GetAllUeServices();
                if (UeServices.Count() == 0) {
                    result.IsSuccessful = false;
                    result.Status = "There are no active UeServices.";

                    return result;
                }


                var UeServiceServerTransferRequests = await _UeServiceRepository.GetAllTransformableUeServiceServerTransferRequests();
                if (UeServiceServerTransferRequests.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = "There are currently no transformable UeService ServerTransferRequests.";

                    return result;
                }

                var UeServiceLatencyRecords = await _UeServiceRepository.GetLatencyRecordsForTransformableServerTransferRequests();
                if (UeServiceLatencyRecords.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = "There are transformable ServerTransferRequests, but no latency records describing their ping map.";

                    return result;
                }

                var zoneNames = UeServiceServerTransferRequests.Select((c) => c.ZoneName!).Distinct();

                var ServerTransferRequestGroupsByUeServiceId = UeServices.ToDictionary((UeService) => UeService.UeServiceId, (UeService) => new List<UeServiceServerTransferRequestGroup>());
                var UeServicesByUeServiceId = UeServices.ToDictionary((UeService) => UeService.UeServiceId, (UeService) => UeService);
                
                var UeServiceLatencyRecordsByAccountId = UeServiceLatencyRecords.ToDictionary((record) => record.AccountId, (record) => new List<mUeServiceLatencyRecord>());
                var UeServiceLatencyRecordsByUeServiceId = UeServiceLatencyRecords.ToDictionary((record) => record.UeServiceId, (record) => new List<mUeServiceLatencyRecord>());

                var relevantZonesByName = (await _zoneRepository.GetAllZones())
                    .Where((zone) => zoneNames.Contains(zone.ZoneName))
                    .ToDictionary((zone) => zone.ZoneName!, (zone) => zone);

                var ServerTransferRequestsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName!, (zoneName) => new List<MUeServiceServerTransferRequest>());

                foreach (var ServerTransferRequest in UeServiceServerTransferRequests)
                    ServerTransferRequestsByZoneName[ServerTransferRequest.ZoneName].Add(ServerTransferRequest);

                foreach (var record in UeServiceLatencyRecords)
                {
                    UeServiceLatencyRecordsByAccountId[record.AccountId].Add(record);
                    UeServiceLatencyRecordsByUeServiceId[record.UeServiceId].Add(record);
                }

                foreach (var zoneName in zoneNames)
                {
                    
                    var zone = relevantZonesByName[zoneName];
                    var ServerTransferRequests = ServerTransferRequestsByZoneName[zoneName];

                    var ServerTransferRequestsWithParties = ServerTransferRequests.Where((ServerTransferRequest) => ServerTransferRequest.PartyId.HasValue);
                    var ServerTransferRequestsByPartyId = ServerTransferRequestsWithParties.ToDictionary((ServerTransferRequest) => ServerTransferRequest.PartyId!.Value, (ServerTransferRequest) => new List<MUeServiceServerTransferRequest>());

                    foreach (var partyServerTransferRequest in ServerTransferRequestsWithParties)
                        ServerTransferRequestsByPartyId[partyServerTransferRequest.PartyId!.Value].Add(partyServerTransferRequest);

                    var partyMappings = new Stack<UeServicePartyMapping>(
                        ServerTransferRequestsByPartyId.Values
                            .Where((partyServerTransferRequests) => partyServerTransferRequests.Count > 1)
                            .Select((partyServerTransferRequests) =>
                            {
                                var partyMapping = new UeServicePartyMapping
                                {
                                    ZoneName = zoneName,
                                    ServerTransferRequests = partyServerTransferRequests,
                                    AverageLatencyByUeServiceId = new Dictionary<Guid, float>(),
                                    AverageLevel = partyServerTransferRequests.Average((ServerTransferRequest) => ServerTransferRequest.CharacterLevel)
                                };

                                foreach (var UeService in UeServices)
                                {
                                    var avgLatency = UeServiceLatencyRecords
                                        .Where((record) => partyServerTransferRequests.Exists((ctr) => ctr.AccountId == record.AccountId))
                                        .Select((record) => record.Latency)
                                        .Average();

                                    partyMapping.AverageLatencyByUeServiceId.Add(UeService.UeServiceId, avgLatency);
                                }

                                return partyMapping;
                            })
                            .OrderByDescending((partyMapping) => partyMapping.ServerTransferRequests.Count)
                            .ToArray());

                    // Assign parties first, creating groups as necessary

                    while (partyMappings.Count > 0)
                    {
                        var party = partyMappings.Pop();

                        foreach (var latencyLimit in SERVER_LATENCY_TIERS)
                        {
                            var possibleUeServicesForLatencyTier = UeServices
                                .Where((UeService) => party.AverageLatencyByUeServiceId[UeService.UeServiceId] <= latencyLimit)
                                .OrderBy((UeService) => party.AverageLatencyByUeServiceId[UeService.UeServiceId]);

                            if (possibleUeServicesForLatencyTier.Count() == 0)
                                continue;

                            var possibleExistingGroupsForLatencyTier = ServerTransferRequestGroupsByUeServiceId
                                .Where((kv) =>
                                    possibleUeServicesForLatencyTier.Any((UeService) => UeService.UeServiceId == kv.Key) &&
                                    kv.Value.Any((group) => 
                                        zone.ZoneName == zoneName &&
                                        zone.HardPlayerCap - group.NumberOfPlayers >= party.ServerTransferRequests.Count));

                            // If there are no groups that will fit this party, we need to make a new one.
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var targetUeService = possibleUeServicesForLatencyTier
                                    .FirstOrDefault((UeService) => UeService.NumberOfFreeEntries > 0);

                                // There are no UeServices with space for a free entry, we'll have to go to a higher latency tier
                                if (targetUeService == null)
                                    continue;

                                // Create the new ServerTransferRequest group for the target UeService
                                var newServerTransferRequestGroup = new UeServiceServerTransferRequestGroup
                                {
                                    ServerTransferRequests = party.ServerTransferRequests,
                                    UeServiceId = targetUeService.UeServiceId,
                                    UeServiceServerTransferRequestGroupId = Guid.NewGuid(),
                                    NumberOfParties = 1,
                                    NumberOfPlayers = (byte)party.ServerTransferRequests.Count,
                                    ZoneName = zoneName
                                };

                                // Add the ServerTransferRequest group to the list for this UeService
                                ServerTransferRequestGroupsByUeServiceId[targetUeService.UeServiceId].Add(newServerTransferRequestGroup);

                                // Remove the ServerTransferRequests referenced by this party so they don't get processed again later
                                ServerTransferRequests.RemoveAll((ServerTransferRequest) => party.ServerTransferRequests.Any((pc) => pc.UeServiceServerTransferRequestId == ServerTransferRequest.UeServiceServerTransferRequestId));
                                
                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping, then by closest in avg level, then by least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .SelectMany((kv) => kv.Value)
                                .Where((group) => group.ZoneName == zoneName)
                                .OrderBy((group) => party.AverageLatencyByUeServiceId[group.UeServiceId]) // Order by lowest ping first
                                .ThenBy((group) => Math.Abs(group.ServerTransferRequests.Average((ctr) => ctr.CharacterLevel) - party.AverageLevel)) // Then try to match to a group with the lowest avg level diff
                                .ThenBy((group) => group.NumberOfPlayers) // Then try to put the party into the least populated server
                                .First(); // This should never throw, if it does then something is seriously wrong.

                            group.NumberOfParties++;
                            group.NumberOfPlayers += (byte)party.ServerTransferRequests.Count;
                            group.ServerTransferRequests.AddRange(party.ServerTransferRequests);

                            ServerTransferRequests.RemoveAll((ServerTransferRequest) => party.ServerTransferRequests.Any((pc) => pc.UeServiceServerTransferRequestId == ServerTransferRequest.UeServiceServerTransferRequestId));

                            break;
                        }
                    }

                    // Parties have been assigned, assign individual ServerTransferRequests now
                    var remainingServerTransferRequests = new Stack<MUeServiceServerTransferRequest>(ServerTransferRequests);

                    while (remainingServerTransferRequests.Count > 0)
                    {
                        var currentServerTransferRequest = remainingServerTransferRequests.Pop();

                        foreach (var latencyLimit in SERVER_LATENCY_TIERS)
                        {
                            var latencyRecordsForTier = UeServiceLatencyRecordsByAccountId[currentServerTransferRequest.AccountId]
                                .Where((record) => record.Latency <= latencyLimit)
                                .OrderBy((record) => record.Latency);


                            // There are no latency records that fit inside of this tier, move to a higher tier.
                            if (latencyRecordsForTier.Count() == 0)
                                continue;



                            var possibleExistingGroupsForLatencyTier = ServerTransferRequestGroupsByUeServiceId
                                .Where((kv) =>
                                    latencyRecordsForTier.Any((record) => record.UeServiceId == kv.Key) &&
                                    kv.Value.Any((group) =>
                                        zone.ZoneName == zoneName &&
                                        zone.SoftPlayerCap - group.NumberOfPlayers > 0)); // Use the soft player cap to try to maintain server performance

                            // If there are no groups that will fit this party, we need to make a new one.
                            // We should also select a UeService that has the lowest possible ping for the remaining ServerTransferRequests
                            // Because it's very likely that we will be sending further ServerTransferRequests over to this group
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var possibleTargets = latencyRecordsForTier
                                    .Where((UeService) => UeServicesByUeServiceId[UeService.UeServiceId].NumberOfFreeEntries > 0);

                                // There are no UeServices with space for a free entry, we'll have to go to a higher latency tier
                                if (possibleTargets.Count() == 0)
                                    continue;



                                var bestPossibleTarget = possibleTargets
                                    .OrderBy((r) => remainingServerTransferRequests
                                        .Select((ServerTransferRequest) =>
                                            UeServiceLatencyRecordsByAccountId[ServerTransferRequest.AccountId]
                                                .FirstOrDefault((record) => record.UeServiceId == r.UeServiceId))
                                        .Where((latencyRecord) =>
                                            latencyRecord != null &&
                                            latencyRecord.Latency <= latencyLimit)
                                        .Average((record) => record.Latency))
                                    .FirstOrDefault();

                                // Couldn't find a target UeService
                                if (bestPossibleTarget == null)
                                    continue;


                                // Create the new ServerTransferRequest group for the target UeService
                                var newServerTransferRequestGroup = new UeServiceServerTransferRequestGroup
                                {
                                    ServerTransferRequests = new List<MUeServiceServerTransferRequest>() { currentServerTransferRequest },
                                    UeServiceId = bestPossibleTarget.UeServiceId,
                                    UeServiceServerTransferRequestGroupId = Guid.NewGuid(),
                                    NumberOfParties = 0,
                                    NumberOfPlayers = 1,
                                    ZoneName = zoneName
                                };

                                // Add the ServerTransferRequest group to the list for this UeService
                                ServerTransferRequestGroupsByUeServiceId[bestPossibleTarget.UeServiceId].Add(newServerTransferRequestGroup);
                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping and least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .SelectMany((kv) => kv.Value)
                                .Where((group) => group.ZoneName == zoneName)
                                .OrderBy((group) => UeServiceLatencyRecordsByUeServiceId[group.UeServiceId].First((r) => r.AccountId == currentServerTransferRequest.AccountId).Latency) // Sort by lowest ping
                                .ThenBy((group) => Math.Abs(group.ServerTransferRequests.Average((ctr) => ctr.CharacterLevel) - currentServerTransferRequest.CharacterLevel)) // Then try to match to a group with the lowest avg level diff
                                .ThenByDescending((group) => group.NumberOfPlayers) // Try to put the player into a populated server
                                .First(); // This should never throw, if it does then something is seriously wrong.

                            group.NumberOfPlayers++;
                            group.ServerTransferRequests.Add(currentServerTransferRequest);

                            break;
                        }
                    }
                }



               


                result.IsSuccessful = true;
                result.ServerTransferRequestGroups = ServerTransferRequestGroupsByUeServiceId.Values.SelectMany((x) => x).ToList();
                result.Status = $"Successfully created {result.ServerTransferRequestGroups.Count} ServerTransferRequest groups.";



                return result;
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);

                return AServiceCallResult.BuildForException<RGetUeServiceServerTransferRequestGroupsResult>(ex); ;
            }
            finally
            {
                // _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }


        public async Task<ServiceCallResult> ProcessUeServiceServerTransferRequestGroups(List<UeServiceServerTransferRequestGroup> UeServiceServerTransferRequestGroups)
        {
            var result = new ServiceCallResult { };

            try
            {
                _gsUnitOfWork.BeginTransaction();

                foreach (var ServerTransferRequestGroup in UeServiceServerTransferRequestGroups)
                {
                    await _UeServiceRepository.ProcessUeServiceServerTransferRequestGroup(
                        string.Join(",", ServerTransferRequestGroup.ServerTransferRequests.Select((ServerTransferRequest) => ServerTransferRequest.UeServiceServerTransferRequestId.ToString("D"))),
                        ServerTransferRequestGroup.UeServiceId,
                        ServerTransferRequestGroup.UeServiceServerTransferRequestGroupId,
                        ServerTransferRequestGroup.ZoneName);
                }


                result.IsSuccessful = true;
                result.Status = $"Successfully inserted {UeServiceServerTransferRequestGroups.Count} UeService ServerTransferRequest groups";

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


        public async Task<ServiceCallResult> BeginProcessingTransformedUeServiceServerTransferRequests()
        {
            // TODO: NEED TO GROUP ServerTransferRequestS ON THEIR ServerTransferRequest GROUP ID FIELD
            // TODO: NEED TO MAKE A SERVER INSTANCE FOR EACH ServerTransferRequest GROUP
            // TODO: NEED TO ASSIGN SERVER INSTANCE A CHANNEL ID AND PUT IT INTO THE POOL OF SERVERS
            // TODO: NEED TO UPDATE ServerTransferRequest OBJECTS IN THE DATABASE WITH THE CHANNEL ID

            if (!CreateResultAndCheckUeServiceState(out ServiceCallResult result))
                return result;

            var UeServiceId = _UeServiceState.GetUeServiceId()!.Value;

            try
            {
                _gsUnitOfWork.BeginTransaction();

                var ServerTransferRequests = await _UeServiceRepository.GetAllTransformedUeServiceServerTransferRequests(UeServiceId);
                if (ServerTransferRequests.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = $"There are currently no transformed ServerTransferRequests for UeServiceId {UeServiceId}";

                    return result;
                }

                var ServerTransferRequestsByGroupId = ServerTransferRequests.GroupBy((ServerTransferRequest) => ServerTransferRequest.UeServiceServerTransferRequestId);

                // TODO: SPIN UP A SERVER FOR EACH GROUP
                // TODO: CREATE A CHANNEL IN THE DATABASE FOR EACH GROUP
                // TODO: UPDATE EACH GROUP WITH THE CHANNEL ID

                return null;

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

        public async Task<ServiceCallResult> ProcessPendingUeServiceServerTransferRequests()
        {
            if (!CreateResultAndCheckUeServiceState(out ServiceCallResult result))
                return result;

            var UeServiceId = _UeServiceState.GetUeServiceId()!.Value;

            try
            {
                _gsUnitOfWork.BeginTransaction();


                var ServerTransferRequestsToProcess = await _UeServiceRepository.BeginProcessingPendingServerTransferRequests(UeServiceId);
                if (ServerTransferRequestsToProcess.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = $"No ServerTransferRequests to process for UeService {UeServiceId}";

                    return result;
                }

                var channels = await _channelRepository.GetAllChannels();
                if (channels.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = $"There are no channels to load the ServerTransferRequests into!";

                    return result;
                }


                // TODO: QUEUE IPC SEND TO UNREAL SERVER INSTANCE
                //

                foreach (var ServerTransferRequest in ServerTransferRequestsToProcess)
                {
                    var serverInstance = _UeServiceState.ServersByChannelId[ServerTransferRequest.TargetChannelId!.Value];
                    if (serverInstance == null)
                        throw new Exception($"Received ServerTransferRequest on UeService {UeServiceId} with invalid channel id {ServerTransferRequest.TargetChannelId.Value}");


                }




                await _UeServiceRepository.FinishProcessingPendingServerTransferRequests(UeServiceId);

                result.IsSuccessful = true;
                result.Status = $"Successfully processed and removed {ServerTransferRequestsToProcess.Count()} completed ServerTransferRequests from the database.";

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
