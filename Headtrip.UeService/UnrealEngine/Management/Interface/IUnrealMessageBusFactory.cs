using Headtrip.UeService.UnrealEngine.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Management.Interface
{
    public interface IUnrealMessageBusFactory
    {

        IUnrealMessageBus Create(IUnrealServerInstance ServerInstance);
        IEnumerable<IUnrealMessagePoller> GetPollers();
        Task DestroyMessageBus(IUnrealMessageBus MessageBus);
        IUnrealMessageBus? GetByServerId(Guid ServerId);
        Task Shutdown();

    }
}
