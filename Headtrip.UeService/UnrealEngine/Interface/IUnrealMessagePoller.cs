using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Interface
{
    public interface IUnrealMessagePoller
    {
        Task Poll(CancellationToken Token);
    }
}
