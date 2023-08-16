using Headtrip.UeMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.UnrealEngine.Interface
{
    public interface IUnrealMessageBus
    {
        string Port { get; }
        Guid MessageBusId { get; }
        Task SendMessage(AUnrealMessage Message);

    }
}
