using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Repositories.Implementation.GameServer;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UeMessages.Inbound;
using Headtrip.UeService.Models;
using Headtrip.UeService.Objects.Results.Abstract;
using Headtrip.UeService.State;
using Headtrip.UeService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UeService.UnrealEngine.MessageHandlers.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.MessageHandlers
{
    public sealed class HMsgServerStartup : AUnrealMessageHandler<MsgServerStartup>, IUnrealMessageHandler
    {
        private sealed class RCreateChannelResult : ATaskResult
        {
            public MChannel? Channel { get; set; }
        }


        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IUnitOfWork<HeadtripGameServerContext> _GsUnitOfWork;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IUeStrRepository _UeStrRepository;


        public HMsgServerStartup(
            ILogging<HeadtripGameServerContext> Logging,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork,
            IChannelRepository ChannelRepository,
            IUeStrRepository UeStrRepository) : base(MsgServerStartup.MsgType)
        {
            _Logging = Logging;
            _GsUnitOfWork = gsUnitOfWork;
            _ChannelRepository = ChannelRepository;
            _UeStrRepository = UeStrRepository;
        }

        private async Task<RCreateChannelResult> CreateChannel(string ConnectionString, TStrGroup Group)
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
                    ConnectionString = ConnectionString,
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

        protected async override Task HandleMessage(MsgServerStartup Message, CancellationToken Token)
        {
            // FINISH CREATING THE SERVER
            // CREATE CHANNEL WITH CONNECTION STRING
            // CREATE MAPPING TO SERVER WITH CHANNEL ID
            // COMPLETE STRS

            if (!UeServiceState.ActiveServersByStrGroupId.TryGetValue(_ServerInstance.ProgenitorGroup.GroupId, out var serverDescriptor))
                throw new Exception($"Unable to lookup server descriptor for str group {_ServerInstance.ProgenitorGroup.GroupId}");


            var channelResult = await CreateChannel(Message.ConnectionString, _ServerInstance.ProgenitorGroup);
            if (!channelResult.IsSuccessful)
                throw new Exception($"Unable to create channel for group {_ServerInstance.ProgenitorGroup.GroupId}");

            serverDescriptor.Channel = channelResult.Channel;

            UeServiceState.ActiveServersByStrGroupId[_ServerInstance.ProgenitorGroup.GroupId] = serverDescriptor;
            UeServiceState.ActiveServersByChannelId[channelResult.Channel.ChannelId] = serverDescriptor;

            await CompleteServerTransferRequests(_ServerInstance.ProgenitorGroup, serverDescriptor.Channel.ChannelId);
        }

    }
}
