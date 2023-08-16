using Headtrip.GameServerContext;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgPlayerLeave : AUnrealMessageHandler<MsgPlayerLeave>, IUnrealMessageHandler
    {
        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IContext<HeadtripGameServerContext> _Context;
        private readonly IChannelRepository _ChannelRepository;


        public HMsgPlayerLeave(
            ILogging<HeadtripGameServerContext> Logging,
            IContext<HeadtripGameServerContext> Context,
            IChannelRepository ChannelRepository) :
        base(
            MsgPlayerLeave.MsgType)
        {
            _Logging = Logging;
            _Context = Context;
            _ChannelRepository = ChannelRepository;
        }

        protected async override Task HandleMessage(MsgPlayerLeave Message, CancellationToken Token)
        {
            try
            {
                using (var transaction = _Context.BeginTransaction())
                {
                    await _ChannelRepository.DecrementPlayerCount(_ServerInstance.ChannelId!.Value);

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex, $"Error while decrementing channel player count for server {_ServerInstance.ServerId}");
            }
        }
    }
}
