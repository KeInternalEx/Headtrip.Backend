using Headtrip.UeService.UnrealEngine.Messaging.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Abstract
{
    public interface IUnrealServerInstance : IDisposable, IAsyncDisposable
    {
        Task WriteString(string text);
        Task WriteObject(IMessageObject messageObject);
        void AddMessageHandler<T>(MessageHandler handler);
        void RemoveMessageHandler<T>(MessageHandler handler);
    }
}
