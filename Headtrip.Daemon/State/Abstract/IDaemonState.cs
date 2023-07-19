using Headtrip.Daemon.Models;
using Headtrip.Models.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.State.Abstract
{
    public interface IDaemonState
    {
        Dictionary<Guid, Channel> ChannelsByChannelId { get; }
        Dictionary<Guid, UnrealServerInstance> ServersByChannelId { get; }








        bool IsReady();
        bool? IsSuperDaemon();
        string? GetDaemonNickname();
        Guid? GetDaemonId();
        Task<bool> Initialize();

    }
}
