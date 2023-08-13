using Headtrip.UeMessages;
using Headtrip.UeService.UnrealEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.MessageHandlers.Interface
{
    public interface IUnrealMessageHandler
    {
        Task HandleMessage(AUnrealMessage Message, CancellationToken Token);
        void SetServerInstance(IUnrealServerInstance ServerInstance);
    }
}
