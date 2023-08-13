using Headtrip.Objects.UeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Interface
{
    public interface IUnrealServerInstance
    {
        Guid? ChannelId { get; }
        Guid ServerId { get; }
        string LevelName { get; }
        TStrGroup ProgenitorGroup { get; }
    }
}
