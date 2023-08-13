using Headtrip.UeService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UeService.UnrealEngine.MessageHandlers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgPlayerLeave : AUnrealMessageHandler<MsgPlayerLeave>, IUnrealMessageHandler
    {

        public HMsgPlayerLeave() : base(MsgPlayerLeave.MsgType) { }
        protected async override Task HandleMessage(MsgPlayerLeave Message, CancellationToken Token)
        {
            // TODO: NEED TO SEND THIGNG

        }
    }
}
