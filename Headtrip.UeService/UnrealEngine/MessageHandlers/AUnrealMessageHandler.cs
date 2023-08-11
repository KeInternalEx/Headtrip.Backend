using Headtrip.UeMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.MessageHandlers
{
    public abstract class AUnrealMessageHandler<TMessage> where TMessage : AUnrealMessage
    {
        public readonly string MsgType;

        protected AUnrealMessageHandler(string msgType)
            => MsgType = msgType;

        protected abstract Task HandleMessage(TMessage Message);
        public async Task HandleMessage(AUnrealMessage Message)
        {
            if (Message.Type == MsgType)
                await HandleMessage(Message as TMessage);
        }
    }
}
