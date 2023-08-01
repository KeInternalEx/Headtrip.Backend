global using MessageHandler = System.Action<Headtrip.UeService.UnrealEngine.Messaging.Abstract.IMessageObject>;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Messaging.Abstract
{

    public interface IMessagePipeline
    {
        void AddMessageHandler<T>(MessageHandler handler);
        void RemoveMessageHandler<T>(MessageHandler handler);
        int ProcessMessage(byte[] message);
    }
}
