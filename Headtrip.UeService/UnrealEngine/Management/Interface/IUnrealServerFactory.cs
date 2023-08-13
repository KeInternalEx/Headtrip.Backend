﻿using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.UeService.UnrealEngine.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Management.Interface
{

    public interface IUnrealServerFactory
    {
        IUnrealServerInstance Create(string LevelName, TStrGroup ProgenitorGroup);

        Task StartInstance(IUnrealServerInstance Instance);
        Task StopInstance(IUnrealServerInstance Instance);

        IUnrealServerInstance? GetByChannelId(Guid ChannelId);
        IUnrealServerInstance? GetByProgenitorGroupId(Guid GroupId);

        void SetChannel(IUnrealServerInstance Instance, MChannel Channel);

        void Shutdown();
    }
}
