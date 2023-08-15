using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Objects.UnrealService;
using Headtrip.Objects.UnrealService.Transient;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UeMessages.Inbound;
using Headtrip.UnrealService.Objects.Results.Abstract;
using Headtrip.UnrealService.Objects.UnrealServer;
using Headtrip.UnrealService.State;
using Headtrip.UnrealService.UnrealEngine.Management.Interface;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Interface;
using Headtrip.Utilities.Interface;

namespace Headtrip.UnrealService.UnrealEngine.MessageHandlers
{
    public sealed class HMsgServerStartup : AUnrealMessageHandler<MsgServerStartup>, IUnrealMessageHandler
    {
        private sealed class RCreateChannelResult : ATaskResult
        {
            public MChannel? Channel { get; set; }
        }


        private readonly IContext<HeadtripGameServerContext> _Context;
        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IUnrealStrRepository _UeStrRepository;
        private readonly IUnrealServiceRepository _UnrealServiceRepository;
        private readonly IUnrealServerFactory _UnrealServerFactory;

        public HMsgServerStartup(
            IContext<HeadtripGameServerContext> Context,
            ILogging<HeadtripGameServerContext> Logging,
            IChannelRepository ChannelRepository,
            IUnrealStrRepository UeStrRepository,
            IUnrealServiceRepository UnrealServiceRepository,
            IUnrealServerFactory UnrealServerFactory) : base(MsgServerStartup.MsgType)
        {
            _Context = Context;
            _Logging = Logging;
            _ChannelRepository = ChannelRepository;
            _UeStrRepository = UeStrRepository;
            _UnrealServiceRepository = UnrealServiceRepository;
            _UnrealServerFactory = UnrealServerFactory;
        }


        private async Task UpdateRemainingServersCounter()
        {
            UnrealServiceState.ServiceModel.Update((m) => m.NumberOfFreeEntries--);
            await _UnrealServiceRepository.Update(UnrealServiceState.ServiceModel.Value);
        }

        private async Task<RCreateChannelResult> CreateChannel(string ConnectionString, TStrGroup Group, Guid ServerId)
        {
            var result = new RCreateChannelResult
            {
                IsSuccessful = false
            };


            result.Channel = await _ChannelRepository.Create(new MChannel
            {
                UnrealServiceId = UnrealServiceState.ServiceId,
                UnrealServerId = ServerId,
                ConnectionString = ConnectionString,
                IsAvailable = false,
                ZoneName = Group.ZoneName,
                NumberOfPlayers = Group.NumberOfPlayers
            });

            result.IsSuccessful = true;
            result.Status = $"Successfully created channel on zone {Group.ZoneName} w/ ID {result.Channel.ChannelId} for grp {Group.GroupId}";

            return result;
        }

        private async Task<RUnrealServiceResult> CompleteServerTransferRequests(TStrGroup Group, Guid ChannelId)
        {
            var result = new RUnrealServiceResult
            {
                IsSuccessful = false
            };

            foreach (var str in Group.ServerTransferRequests)
            {
                str.TargetChannelId = ChannelId;
                str.TargetUnrealServiceId = UnrealServiceState.ServiceId;
                str.State = EUeServerTransferRequestState.PendingAssignment;
            }

            await _UeStrRepository.BulkUpdate(Group.ServerTransferRequests);

            result.IsSuccessful = true;
            result.Status = $"Successfully updated {Group.ServerTransferRequests.Count} strs for group {Group.GroupId}";

            return result;
        }



        private async Task<MChannel> CompleteServerStartup(MsgServerStartup Message)
        {
            using (var transaction = _Context.BeginTransaction())
            {

                var channelResult = await CreateChannel(Message.ConnectionString, _ServerInstance.ProgenitorGroup, _ServerInstance.ServerId);
                if (!channelResult.IsSuccessful)
                    throw new Exception($"Unable to create channel for group {_ServerInstance.ProgenitorGroup.GroupId}");


                await CompleteServerTransferRequests(_ServerInstance.ProgenitorGroup, channelResult.Channel.ChannelId);
                await UpdateRemainingServersCounter();


                transaction.Complete();

                return channelResult.Channel;
            }
        }


        protected async override Task HandleMessage(MsgServerStartup Message, CancellationToken Token)
        {
            try
            {
                _UnrealServerFactory.SetChannel(_ServerInstance, await CompleteServerStartup(Message));
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex, $"Error while starting server for group {_ServerInstance.ProgenitorGroup.GroupId}");

                using (var transaction = _Context.BeginTransaction())
                {
                    foreach (var str in _ServerInstance.ProgenitorGroup.ServerTransferRequests)
                        str.State = EUeServerTransferRequestState.FailedServerCreation;

                    await _UeStrRepository.BulkUpdate(_ServerInstance.ProgenitorGroup.ServerTransferRequests);
                }

                await _UnrealServerFactory.StopInstance(_ServerInstance);
            }
        }

    }
}
