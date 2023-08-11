using Headtrip.UeService.UnrealEngine.MessageHandlers;
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
        protected async override Task HandleMessage(MsgPlayerLeave Message)
        {
            // TODO: NEED TO SEND THIGNG

        }
    }
}
