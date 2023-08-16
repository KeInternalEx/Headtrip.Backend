using Headtrip.Objects.UnrealService.Transient;
using Headtrip.UeMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.UnrealEngine.Interface
{
    public interface IUnrealServerInstance
    {
        Guid? ChannelId { get; }
        Guid ServerId { get; }
        string LevelName { get; }
        string ZoneName { get; }
        TStrGroup ProgenitorGroup { get; }

        Task SendMessage(AUnrealMessage Message);
    }
}
