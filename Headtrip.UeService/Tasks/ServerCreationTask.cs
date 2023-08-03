using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UeService.Models;
using Headtrip.UeService.Models.Abstract.Results;
using Headtrip.UeService.Objects;
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

        


        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IUnitOfWork<HeadtripGameServerContext> _GsUnitOfWork;

        private readonly IUeServiceRepository _UeServiceRepository;
        private readonly IUeStrRepository _UeStrRepository;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IZoneRepository _ZoneRepository;


        public ServerCreationTask(
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
       
        private async Task<RCreateChannelResult> CreateChannel(UnrealServerInstance ServerInstance, TStrGroup Group)
        {
            var result = new RCreateChannelResult
            {
                IsSuccessful = false
            };

            _GsUnitOfWork.BeginTransaction();

            try
            {
                result.Channel = await _ChannelRepository.Create(new MChannel
                {
                    UeServiceId = UeServiceState.ServiceId,
                    ConnectionString = ServerInstance.ConnectionString,
                    IsAvailable = false,
                    ZoneName = Group.ZoneName,
                    NumberOfPlayers = Group.NumberOfPlayers
                });

                result.IsSuccessful = true;
                result.Status = $"Successfully created channel on zone {Group.ZoneName} w/ ID {result.Channel.ChannelId} for grp {Group.GroupId}";

                return result;
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                return ATaskResult.BuildForException<RCreateChannelResult>(ex);
            }
            finally
            {
                _GsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }
        private async Task DeleteChannel(Guid ChannelId)
        {
            _GsUnitOfWork.BeginTransaction();

            try
            {
                await _ChannelRepository.Delete(ChannelId);

                _GsUnitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                _GsUnitOfWork.RollbackTransaction();
            }
        }

        private async Task<RUeServiceResult> CompleteServerTransferRequests(TStrGroup Group, Guid ChannelId)
        {
            var result = new RUeServiceResult
            {
                IsSuccessful = false
            };

            _GsUnitOfWork.BeginTransaction();

            try
            {
                foreach (var str in Group.ServerTransferRequests)
                {
                    str.TargetChannelId = ChannelId;
                    str.TargetUeServiceId = UeServiceState.ServiceId;
                    str.State = EUeServerTransferRequestState.PendingAssignment;
                }

                await _UeStrRepository.BulkUpdate(Group.ServerTransferRequests);

                result.IsSuccessful = true;
                result.Status = $"Successfully updated {Group.ServerTransferRequests.Count} strs for group {Group.GroupId}";

                return result;
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                return ATaskResult.BuildForException<RUeServiceResult>(ex);
            }
            finally
            {
                _GsUnitOfWork.Finalize(result.IsSuccessful);
            }
        }

        private async Task<bool> ProcessGroup(TCreationGroup group)
        {
            UnrealServerInstance serverInstance;

            try
            {
                serverInstance = new UnrealServerInstance(group.LevelName);
                await serverInstance.Begin();
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                _Logging.LogWarning($"Error while creating server for group {group.Group.GroupId} / {group.LevelName}");

                return false;
            }

            try
            {
                var channelCreationResult = await CreateChannel(serverInstance, group.Group);
                if (!channelCreationResult.IsSuccessful)
                    throw new Exception($"Unable to create channel for group {group.Group.GroupId}");

                try
                {
                    await serverInstance.SetChannelId(channelCreationResult.Channel.ChannelId);


                    var ueServerDescriptor = new TUeServer
                    {
                        InitialGroup = group.Group,
                        Channel = channelCreationResult.Channel,
                        Instance = serverInstance
                    };


                    if (!UeServiceState.ActiveServersByChannelId.TryAdd(
                        channelCreationResult.Channel.ChannelId,
                        ueServerDescriptor))
                    {
                        throw new Exception($"Unable to add server to collection for channel {channelCreationResult.Channel.ChannelId}");
                    }

                    try
                    {
                        if (!UeServiceState.ActiveServersByStrGroupId.TryAdd(
                            group.Group.GroupId,
                            ueServerDescriptor))
                        {
                            throw new Exception($"Unable to add server to collection for group {group.Group.GroupId} - cid {channelCreationResult.Channel.ChannelId}");
                        }


                        try
                        {
                            var success = await CompleteServerTransferRequests(group.Group, channelCreationResult.Channel.ChannelId);
                            if (success.IsSuccessful)
                            {
                                return true;
                            }

                            throw new Exception($"Unable to update STR states for group {group.Group.GroupId}");
                        }
                        catch (Exception)
                        {
                            UeServiceState.ActiveServersByChannelId.TryRemove(channelCreationResult.Channel.ChannelId, out var _dummy);
                            UeServiceState.ActiveServersByStrGroupId.TryRemove(group.Group.GroupId, out _dummy);

                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        UeServiceState.ActiveServersByChannelId.TryRemove(channelCreationResult.Channel.ChannelId, out var _dummy);
                        throw;
                    }

                }
                catch (Exception) // Manually roll back channel creation and rethrow the exception
                {
                    await DeleteChannel(channelCreationResult.Channel.ChannelId);
                    throw;
                }

            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                await serverInstance.DisposeAsync();
                return false;
            }


            // create channel based on this instance DONE
            // update channel id for the server instance (REQUIRES IMPLEMENTATION)
            // update state dictionaries with pointer to server instance DONE
            // update transfer requests with the assignment pending flag DONE
        }

        public async Task UpdateRemainingServersCounter(int NumberOfServersCreated)
        {
            _GsUnitOfWork.BeginTransaction();

            try
            {
                UeServiceState.ServiceModel.Update((m) => m.NumberOfFreeEntries -= NumberOfServersCreated);
                await _UeServiceRepository.Update(UeServiceState.ServiceModel.Value);

                _GsUnitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                _GsUnitOfWork.RollbackTransaction();
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
                        var numberOfServersCreated = 0;

                        foreach (var group in groupsResult.Groups)
                        {
                            var success = await ProcessGroup(group);
                            if (!success)
                            {
                                _Logging.LogWarning($"Unable to create server for STR group {group.Group.GroupId}");
                                // TODO: mark the contracts in this group as FAILED
                                continue;
                            }

                            ++numberOfServersCreated;
                        }

                        await UpdateRemainingServersCounter(numberOfServersCreated);
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
