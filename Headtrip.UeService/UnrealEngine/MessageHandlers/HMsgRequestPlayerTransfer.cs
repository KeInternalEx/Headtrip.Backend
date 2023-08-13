using Headtrip.UeService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UeService.UnrealEngine.MessageHandlers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgRequestPlayerTransfer : AUnrealMessageHandler<MsgRequestPlayerTransfer>, IUnrealMessageHandler
    {

        public HMsgRequestPlayerTransfer() : base(MsgRequestPlayerTransfer.MsgType) { }
        protected async override Task HandleMessage(MsgRequestPlayerTransfer Message, CancellationToken Token)
        {
            // TODO: NEED TO SEND THIGNG

        }
    }
}
