using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.UnrealService;
using Headtrip.Objects.UnrealService.Transient;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UnrealService.Objects;
using Headtrip.UnrealService.Objects.Results;
using Headtrip.UnrealService.Objects.Results.Abstract;
using Headtrip.UnrealService.Objects.UnrealServer;
using Headtrip.UnrealService.State;
using Headtrip.UnrealService.Tasks.Abstract;
using Headtrip.UnrealService.Tasks.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.Tasks
{
    public sealed class RequestTransformationTask : AServiceTask, IRequestTransformationTask
    {
        private sealed class RCreateStrGroupsResult : ATaskResult
        {
            public List<TStrGroup>? RequestGroups { get; set; }

        }

        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IContext<HeadtripGameServerContext> _Context;

        private readonly IUnrealServiceRepository _UnrealServiceRepository;
        private readonly IUnrealStrRepository _UeStrRepository;
        private readonly IUnrealLatencyRecordRepository _UeLatencyRecordRepository;
        private readonly IZoneRepository _ZoneRepository;

        public RequestTransformationTask(
            ILogging<HeadtripGameServerContext> Logging,
            IContext<HeadtripGameServerContext> Context,
            IUnrealServiceRepository UnrealServiceRepository,
            IUnrealStrRepository UeStrRepository,
            IUnrealLatencyRecordRepository UeLatencyRecordRepository,
            IZoneRepository ZoneRepository) :
        base(
            UnrealServiceState.CancellationTokenSource.Value.Token,
            UnrealServiceConfiguration.RequestTransformationTaskInterval)
        {
            _Logging = Logging;
            _Context = Context;
            _UnrealServiceRepository = UnrealServiceRepository;
            _UeStrRepository = UeStrRepository;
            _UeLatencyRecordRepository = UeLatencyRecordRepository;
            _ZoneRepository = ZoneRepository;
        }

    
        private async Task<RCreateStrGroupsResult> CreateStrGroups()
        {
            var result = new RCreateStrGroupsResult
            {
                IsSuccessful = false,
                RequestGroups = new List<TStrGroup>()
            };

            try
            {
                var UnrealServices = await _UnrealServiceRepository.ReadAll();
                if (UnrealServices.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = "There are no active UnrealServices.";

                    return result;
                }


                var Requests = await _UeStrRepository.ReadWithState(EUeServerTransferRequestState.PendingTransform);
                if (Requests.Count() == 0)
                {
                    result.IsSuccessful = true;
                    result.Status = "There are currently no transformable ServerTransferRequests.";

                    return result;
                }

                var UnrealServiceLatencyRecords = await _UeLatencyRecordRepository.ReadForTransformableRequests();
                if (UnrealServiceLatencyRecords.Count() == 0)
                {
                    result.IsSuccessful = false;
                    result.Status = "There are transformable ServerTransferRequests, but no latency records describing their ping map.";

                    return result;
                }

                var zoneNames = Requests.Select((c) => c.ZoneName!).Distinct();

                var RequestGroupsByServiceId = UnrealServices.ToDictionary((UnrealService) => UnrealService.UnrealServiceId, (UnrealService) => new List<TStrGroup>());
                var ServicesByServiceId = UnrealServices.ToDictionary((UnrealService) => UnrealService.UnrealServiceId, (UnrealService) => UnrealService);

                var LatencyRecordsByAccountId = UnrealServiceLatencyRecords.ToDictionary((record) => record.AccountId, (record) => new List<MUnrealLatencyRecord>());
                var LatencyRecordsByServiceId = UnrealServiceLatencyRecords.ToDictionary((record) => record.UnrealServiceId, (record) => new List<MUnrealLatencyRecord>());

                var relevantZonesByName = (await _ZoneRepository.ReadAll())
                    .Where((zone) => zoneNames.Contains(zone.ZoneName))
                    .ToDictionary((zone) => zone.ZoneName!, (zone) => zone);

                var RequestsByZoneName = zoneNames.ToDictionary((zoneName) => zoneName!, (zoneName) => new List<MUnrealServerTransferRequest>());

                foreach (var Request in Requests)
                    RequestsByZoneName[Request.ZoneName].Add(Request);

                foreach (var record in UnrealServiceLatencyRecords)
                {
                    LatencyRecordsByAccountId[record.AccountId].Add(record);
                    LatencyRecordsByServiceId[record.UnrealServiceId].Add(record);
                }

                foreach (var zoneName in zoneNames)
                {

                    var zone = relevantZonesByName[zoneName];
                    var ServerTransferRequests = RequestsByZoneName[zoneName];

                    var RequestsWithParties = ServerTransferRequests.Where((ServerTransferRequest) => ServerTransferRequest.PartyId.HasValue);
                    var RequestsByPartyId = RequestsWithParties.ToDictionary((ServerTransferRequest) => ServerTransferRequest.PartyId!.Value, (ServerTransferRequest) => new List<MUnrealServerTransferRequest>());

                    foreach (var partyServerTransferRequest in RequestsWithParties)
                        RequestsByPartyId[partyServerTransferRequest.PartyId!.Value].Add(partyServerTransferRequest);

                    var partyMappings = new Stack<TPartyMapping>(
                        RequestsByPartyId.Values
                            .Where((partyServerTransferRequests) => partyServerTransferRequests.Count > 1)
                            .Select((partyServerTransferRequests) =>
                            {
                                var partyMapping = new TPartyMapping
                                {
                                    ZoneName = zoneName,
                                    ServerTransferRequests = partyServerTransferRequests,
                                    AverageLatencyByUnrealServiceId = new Dictionary<Guid, float>(),
                                    AverageLevel = partyServerTransferRequests.Average((ServerTransferRequest) => ServerTransferRequest.CharacterLevel)
                                };

                                foreach (var UnrealService in UnrealServices)
                                {
                                    var avgLatency = UnrealServiceLatencyRecords
                                        .Where((record) => partyServerTransferRequests.Exists((ctr) => ctr.AccountId == record.AccountId))
                                        .Select((record) => record.Latency)
                                        .Average();

                                    partyMapping.AverageLatencyByUnrealServiceId.Add(UnrealService.UnrealServiceId, avgLatency);
                                }

                                return partyMapping;
                            })
                            .OrderByDescending((partyMapping) => partyMapping.ServerTransferRequests.Count)
                            .ToArray());

                    // Assign parties first, creating groups as necessary

                    while (partyMappings.Count > 0)
                    {
                        var party = partyMappings.Pop();

                        foreach (var latencyLimit in UnrealServiceConstants.SERVER_LATENCY_TIERS)
                        {
                            var possibleUnrealServicesForLatencyTier = UnrealServices
                                .Where((UnrealService) => party.AverageLatencyByUnrealServiceId[UnrealService.UnrealServiceId] <= latencyLimit)
                                .OrderBy((UnrealService) => party.AverageLatencyByUnrealServiceId[UnrealService.UnrealServiceId]);

                            if (possibleUnrealServicesForLatencyTier.Count() == 0)
                                continue;

                            var possibleExistingGroupsForLatencyTier = RequestGroupsByServiceId
                                .Where((kv) =>
                                    possibleUnrealServicesForLatencyTier.Any((UnrealService) => UnrealService.UnrealServiceId == kv.Key) &&
                                    kv.Value.Any((group) =>
                                        zone.ZoneName == zoneName &&
                                        zone.HardPlayerCap - group.NumberOfPlayers >= party.ServerTransferRequests.Count));

                            // If there are no groups that will fit this party, we need to make a new one.
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var targetUnrealService = possibleUnrealServicesForLatencyTier
                                    .FirstOrDefault((UnrealService) => UnrealService.NumberOfFreeEntries > 0);

                                // There are no UnrealServices with space for a free entry, we'll have to go to a higher latency tier
                                if (targetUnrealService == null)
                                    continue;

                                // Create the new ServerTransferRequest group for the target UnrealService
                                var newServerTransferRequestGroup = new TStrGroup
                                {
                                    ServerTransferRequests = party.ServerTransferRequests,
                                    UnrealServiceId = targetUnrealService.UnrealServiceId,
                                    GroupId = Guid.NewGuid(),
                                    NumberOfParties = 1,
                                    NumberOfPlayers = (byte)party.ServerTransferRequests.Count,
                                    ZoneName = zoneName
                                };

                                // Add the ServerTransferRequest group to the list for this UnrealService
                                RequestGroupsByServiceId[targetUnrealService.UnrealServiceId].Add(newServerTransferRequestGroup);

                                // Remove the ServerTransferRequests referenced by this party so they don't get processed again later
                                ServerTransferRequests.RemoveAll((ServerTransferRequest) => party.ServerTransferRequests.Any((pc) => pc.UeServerTransferRequestId == ServerTransferRequest.UeServerTransferRequestId));

                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping, then by closest in avg level, then by least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .SelectMany((kv) => kv.Value)
                                .Where((group) => group.ZoneName == zoneName)
                                .OrderBy((group) => party.AverageLatencyByUnrealServiceId[group.UnrealServiceId]) // Order by lowest ping first
                                .ThenBy((group) => Math.Abs(group.ServerTransferRequests.Average((ctr) => ctr.CharacterLevel) - party.AverageLevel)) // Then try to match to a group with the lowest avg level diff
                                .ThenBy((group) => group.NumberOfPlayers) // Then try to put the party into the least populated server
                                .First(); // This should never throw, if it does then something is seriously wrong.

                            group.NumberOfParties++;
                            group.NumberOfPlayers += (byte)party.ServerTransferRequests.Count;
                            group.ServerTransferRequests.AddRange(party.ServerTransferRequests);

                            ServerTransferRequests.RemoveAll((ServerTransferRequest) => party.ServerTransferRequests.Any((pc) => pc.UeServerTransferRequestId == ServerTransferRequest.UeServerTransferRequestId));

                            break;
                        }
                    }

                    // Parties have been assigned, assign individual ServerTransferRequests now
                    var remainingServerTransferRequests = new Stack<MUnrealServerTransferRequest>(ServerTransferRequests);

                    while (remainingServerTransferRequests.Count > 0)
                    {
                        var currentServerTransferRequest = remainingServerTransferRequests.Pop();

                        foreach (var latencyLimit in UnrealServiceConstants.SERVER_LATENCY_TIERS)
                        {
                            var latencyRecordsForTier = LatencyRecordsByAccountId[currentServerTransferRequest.AccountId]
                                .Where((record) => record.Latency <= latencyLimit)
                                .OrderBy((record) => record.Latency);


                            // There are no latency records that fit inside of this tier, move to a higher tier.
                            if (latencyRecordsForTier.Count() == 0)
                                continue;



                            var possibleExistingGroupsForLatencyTier = RequestGroupsByServiceId
                                .Where((kv) =>
                                    latencyRecordsForTier.Any((record) => record.UnrealServiceId == kv.Key) &&
                                    kv.Value.Any((group) =>
                                        zone.ZoneName == zoneName &&
                                        zone.SoftPlayerCap - group.NumberOfPlayers > 0)); // Use the soft player cap to try to maintain server performance

                            // If there are no groups that will fit this party, we need to make a new one.
                            // We should also select a UnrealService that has the lowest possible ping for the remaining ServerTransferRequests
                            // Because it's very likely that we will be sending further ServerTransferRequests over to this group
                            if (possibleExistingGroupsForLatencyTier.Count() == 0)
                            {
                                var possibleTargets = latencyRecordsForTier
                                    .Where((UnrealService) => ServicesByServiceId[UnrealService.UnrealServiceId].NumberOfFreeEntries > 0);

                                // There are no UnrealServices with space for a free entry, we'll have to go to a higher latency tier
                                if (possibleTargets.Count() == 0)
                                    continue;



                                var bestPossibleTarget = possibleTargets
                                    .OrderBy((r) => remainingServerTransferRequests
                                        .Select((ServerTransferRequest) =>
                                            LatencyRecordsByAccountId[ServerTransferRequest.AccountId]
                                                .FirstOrDefault((record) => record.UnrealServiceId == r.UnrealServiceId))
                                        .Where((latencyRecord) =>
                                            latencyRecord != null &&
                                            latencyRecord.Latency <= latencyLimit)
                                        .Average((record) => record.Latency))
                                    .FirstOrDefault();

                                // Couldn't find a target UnrealService
                                if (bestPossibleTarget == null)
                                    continue;


                                // Create the new ServerTransferRequest group for the target UnrealService
                                var newServerTransferRequestGroup = new TStrGroup
                                {
                                    ServerTransferRequests = new List<MUnrealServerTransferRequest>() { currentServerTransferRequest },
                                    UnrealServiceId = bestPossibleTarget.UnrealServiceId,
                                    GroupId = Guid.NewGuid(),
                                    NumberOfParties = 0,
                                    NumberOfPlayers = 1,
                                    ZoneName = zoneName
                                };

                                // Add the ServerTransferRequest group to the list for this UnrealService
                                RequestGroupsByServiceId[bestPossibleTarget.UnrealServiceId].Add(newServerTransferRequestGroup);
                                break; // Party successfully assigned. Break out of the foreach and allow the next party to be popped from the stack
                            }

                            // Select the best possible group for this party
                            // Lowest ping and least amount of people in the group
                            // 

                            var group = possibleExistingGroupsForLatencyTier
                                .SelectMany((kv) => kv.Value)
                                .Where((group) => group.ZoneName == zoneName)
                                .OrderBy((group) => LatencyRecordsByServiceId[group.UnrealServiceId].First((r) => r.AccountId == currentServerTransferRequest.AccountId).Latency) // Sort by lowest ping
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
                result.RequestGroups = RequestGroupsByServiceId.Values.SelectMany((x) => x).ToList();
                result.Status = $"Created {result.RequestGroups.Count} str groups";


                return result;
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);

                return ATaskResult.BuildForException<RCreateStrGroupsResult>(ex); ;
            }
            finally
            {
                // _gsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }

        private async Task<RUnrealServiceResult> TransformStrGroups(List<TStrGroup> RequestGroups)
        {
            var result = new RUnrealServiceResult
            {
                IsSuccessful = false
            };



            var requests = new List<MUnrealServerTransferRequest>();

            foreach (var group in RequestGroups)
            {
                foreach (var request in group.ServerTransferRequests)
                {
                    request.State = EUeServerTransferRequestState.PendingServerCreation;
                    request.GroupId = group.GroupId;
                    request.TargetUnrealServiceId = group.UnrealServiceId;

                    requests.Add(request);
                }
            }

            await _UeStrRepository.BulkUpdate(requests);

            result.IsSuccessful = true;
            result.Status = $"Successfully tramsformed {RequestGroups.Count} ServerTransferRequest groups";

            return result;


        }


        protected async override Task Execute()
        {
            while (!_Token.IsCancellationRequested)
            {
                try
                {
                    using (var transaction = _Context.BeginTransaction())
                    {
                        var requestGroupsResult = await CreateStrGroups();
                        if (requestGroupsResult.IsSuccessful)
                            await TransformStrGroups(requestGroupsResult.RequestGroups);

                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    _Logging.LogException(ex);
                }
                finally
                {
                    Thread.Sleep(_Interval);
                }
            }
        }


    }
}
