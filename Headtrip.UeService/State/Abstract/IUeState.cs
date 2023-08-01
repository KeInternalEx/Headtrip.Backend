using Headtrip.UeService.Models;
using Headtrip.UeService.UnrealEngine;
using Headtrip.Objects.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.State.Abstract
{
    public interface IUeServiceState
    {
        Dictionary<Guid, Channel> ChannelsByChannelId { get; }
        Dictionary<Guid, UnrealServerInstance> ServersByChannelId { get; }








        bool IsReady();
        bool? IsSuperUeService();
        string? GetUeServiceNickname();
        Guid? GetUeServiceId();
        Task<bool> Initialize();

    }
}
