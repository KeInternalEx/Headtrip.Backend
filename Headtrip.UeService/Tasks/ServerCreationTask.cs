using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UeService.Models;
using Headtrip.UeService.Objects;
using Headtrip.UeService.Objects.Results.Abstract;
using Headtrip.UeService.Objects.UeServer;
using Headtrip.UeService.State;
using Headtrip.UeService.Tasks.Abstract;
using Headtrip.UeService.Tasks.Interface;
using Headtrip.UeService.UnrealEngine;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks
{
    public sealed class ServerCreationTask : AServiceTask, IServerCreationTask
    {
        private struct TCreationGroup
        {
            public TStrGroup Group { get; set; }
            public string LevelName { get; set; }
        }

        private sealed class RGetGroupsPendingCreationResult : ATaskResult
        {
            public List<TCreationGroup>? Groups { get; set; }
        }

        private sealed class RCreateChannelResult : ATaskResult
        {
            public MChannel? Channel { get; set; }
        }



        private readonly IServiceProvider _ServiceProvider;
        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IUnitOfWork<HeadtripGameServerContext> _GsUnitOfWork;

        private readonly IUeServiceRepository _UeServiceRepository;
        private readonly IUeStrRepository _UeStrRepository;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IZoneRepository _ZoneRepository;


        public ServerCreationTask(
            IServiceProvider ServiceProvider,
            ILogging<HeadtripGameServerContext> Logging,
            IUnitOfWork<HeadtripGameServerContext> GsUnitOfWork,
            IUeServiceRepository UeServiceRepository,
            IUeStrRepository UeStrRepository,
            IChannelRepository ChannelRepository,
            IZoneRepository ZoneRepository) :
        base(
            UeServiceState.CancellationTokenSource.Value.Token,
            UeServiceConfiguration.ServerCreationTaskInterval)
        {
            _ServiceProvider = ServiceProvider;
            _Logging = Logging;
            _GsUnitOfWork = GsUnitOfWork;
            _UeServiceRepository = UeServiceRepository;
            _UeStrRepository = UeStrRepository;
            _ChannelRepository = ChannelRepository;
            _ZoneRepository = ZoneRepository;
        }

        private Dictionary<string, string> LevelNameCache =
            new Dictionary<string, string>();

        private async Task<RGetGroupsPendingCreationResult> GetGroupsPendingCreation()
        {
            var result = new RGetGroupsPendingCreationResult
            {
                IsSuccessful = false,
                Groups = new List<TCreationGroup>()
            };

            try
            {
                var serviceId = UeServiceState.ServiceId;
                var requestsPendingCreation = await _UeStrRepository.ReadWithState(EUeServerTransferRequestState.PendingServerCreation);
                var requests = requestsPendingCreation.Where((request) => request.TargetUeServiceId == serviceId);

                if (requests.Count() == 0)
                {
                    result.Status = $"No STRs pending server creation for service id {serviceId}";
                    result.IsSuccessful = true;

                    return result;
                }

                var strGroups = new Dictionary<Guid, TStrGroup>();
                var resultGroups = new List<TCreationGroup>();

                foreach (var request in requests)
                {
                    if (!strGroups.ContainsKey(request.GroupId!.Value))
                    {
                        strGroups.Add(request.GroupId.Value, new TStrGroup
                        {
                            GroupId = request.GroupId.Value,
                            UeServiceId = serviceId,
                            ServerTransferRequests = new List<MUeServerTransferRequest>(),
                            ZoneName = request.ZoneName
                        });
                    }

                    strGroups[request.GroupId.Value].ServerTransferRequests.Add(request);
                    strGroups[request.GroupId.Value].NumberOfPlayers++;
                }

                foreach (var strGroup in strGroups.Values)
                {
                    var levelName = await GetLevelName(strGroup.ZoneName);
                    if (levelName == null)
                    {
                        _Logging.LogWarning($"Unable to get level name for Zone {strGroup.ZoneName}, skipping group {strGroup.GroupId}");
                        continue;
                    }

                    resultGroups.Add(new TCreationGroup
                    {
                        Group = strGroup,
                        LevelName = levelName
                    });
                }

                result.IsSuccessful = true;
                result.Groups = resultGroups;
                result.Status = $"Successfully rebuilt {result.Groups.Count} groups";


                return result;
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                return ATaskResult.BuildForException<RGetGroupsPendingCreationResult>(ex);
            }
        }
        private async Task<string?> GetLevelName(string ZoneName)
        {
            if (LevelNameCache.TryGetValue(ZoneName, out var levelName))
                return levelName;

            try
            {
                levelName = (await _ZoneRepository.Read(ZoneName)).LevelName;
                LevelNameCache.Add(ZoneName, levelName);

                return levelName;
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
            }

            return null;
        }
       
      

        private async Task<bool> ProcessGroup(TCreationGroup group)
        {
            try
            {
                if (UeServiceState.ActiveServersByStrGroupId.TryGetValue(group.Group.GroupId, out var existingServer))
                {
                    return true;
                }

                var serverInstance = new UnrealServerInstance(group.LevelName, group.Group, _Logging);
                var ueServerDescriptor = new TUeServer
                {
                    InitialGroup = group.Group,
                    Instance = serverInstance
                };

                if (!UeServiceState.ActiveServersByStrGroupId.TryAdd(
                    group.Group.GroupId,
                    ueServerDescriptor))
                {
                    throw new Exception($"Unable to add server to collection for group {group.Group.GroupId}");
                }

                serverInstance.Start(_ServiceProvider); // This method actually triggers the rest of the creation process
                                                        // The unreal server will send a message carrying the connection string, which is needed to continue processing

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                _Logging.LogWarning($"Error while creating server for group {group.Group.GroupId} / {group.LevelName}");

                return await Task.FromResult(false);
            }
        }




        protected async override Task Execute()
        {
            while (!_Token.IsCancellationRequested)
            {
                try
                {
                    var groupsResult = await GetGroupsPendingCreation();
                    if (groupsResult.IsSuccessful && groupsResult.Groups.Count > 0)
                    {

                        foreach (var group in groupsResult.Groups)
                        {
                            var success = await ProcessGroup(group);
                            if (!success)
                            {
                                _Logging.LogWarning($"Unable to create server for STR group {group.Group.GroupId}");
                                // TODO: mark the contracts in this group as FAILED
                                continue;
                            }

                        }

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
