using Headtrip.UeMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.MessageHandlers.Abstract
{
    public abstract class AUnrealMessageHandler<TMessage> where TMessage : AUnrealMessage
    {
        public readonly string MsgType;
        protected UnrealServerInstance? _ServerInstance;

        protected AUnrealMessageHandler(string msgType)
            => MsgType = msgType;

        protected abstract Task HandleMessage(TMessage Message, CancellationToken Token);
        public async Task HandleMessage(AUnrealMessage Message, CancellationToken Token)
        {
            if (Message.Type == MsgType)
                await HandleMessage(Message as TMessage, Token);
        }

        public void SetServerInstance(UnrealServerInstance ServerInstance)
            => _ServerInstance = ServerInstance;
    }
}
