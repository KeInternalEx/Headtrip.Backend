using Headtrip.GameServerContext;
using Headtrip.Objects.Instance;
using Headtrip.Objects.UeService;
using Headtrip.UeService.UnrealEngine.Interface;
using Headtrip.UeService.UnrealEngine.Management.Interface;
using Headtrip.Utilities;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Management
{
    public sealed class FUnrealServerFactory : IUnrealServerFactory, IDisposable
    {
        private Dictionary<Guid, TUnrealServerDescriptor>
            _ServerDescriptors = new Dictionary<Guid, TUnrealServerDescriptor>();

        private object _ServerDescriptorLock = new object();
        private bool _Disposing = false;

        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IUnrealMessageBusFactory _UnrealMessageBusFactory;

        public FUnrealServerFactory(
            ILogging<HeadtripGameServerContext> Logging,
            IUnrealMessageBusFactory UnrealMessageBusFactory)
        {
            _Logging = Logging;
            _UnrealMessageBusFactory = UnrealMessageBusFactory;
        }

        public IUnrealServerInstance Create(
            string LevelName,
            TStrGroup ProgenitorGroup)
        {
            if (_Disposing)
                throw new Exception("Cannot create unreal server instance, factory is disposing.");

            var serverInstance = new UnrealServerInstance(LevelName, ProgenitorGroup, _Logging);
            var descriptor = new TUnrealServerDescriptor
            {
                ServerId = serverInstance.ServerId,
                Instance = serverInstance,
                ProgenitorGroup = ProgenitorGroup,
                ServerState = EUnrealServerState.PendingStartup
            };

            lock (_ServerDescriptorLock)
            {
                _ServerDescriptors.Add(serverInstance.ServerId, descriptor);
            }

            return serverInstance;
        }

        public async Task StartInstance(IUnrealServerInstance Instance)
        {
            try
            {
                lock (_ServerDescriptorLock)
                {
                    if (_ServerDescriptors[Instance.ServerId].ServerState != EUnrealServerState.PendingStartup)
                        return;
                }

                var messageBus = _UnrealMessageBusFactory.Create(Instance);

                (Instance as UnrealServerInstance).SetMessageBus(messageBus);
                (Instance as UnrealServerInstance).Start();

                lock (_ServerDescriptorLock)
                {
                    _ServerDescriptors[Instance.ServerId].ServerState = EUnrealServerState.Started;
                }
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);

                lock (_ServerDescriptorLock)
                {
                    _ServerDescriptors[Instance.ServerId].ServerState = EUnrealServerState.Broken;
                }

                await StopInstance(Instance);

                throw;
            }
        }

        public async Task StopInstance(IUnrealServerInstance Instance)
        {
            try
            {
                lock (_ServerDescriptorLock)
                {
                    if (_ServerDescriptors[Instance.ServerId].ServerState == EUnrealServerState.ShuttingDown)
                        return;

                    _ServerDescriptors[Instance.ServerId].ServerState = EUnrealServerState.ShuttingDown;
                }

                var messageBus = _UnrealMessageBusFactory.GetByServerId(Instance.ServerId);
                if (messageBus != null)
                    await _UnrealMessageBusFactory.DestroyMessageBus(messageBus);


                (Instance as UnrealServerInstance).Dispose();

                lock (_ServerDescriptorLock)
                {
                    _ServerDescriptors.Remove(Instance.ServerId);
                }
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex, $"Fatal exception while attempting to terminate unreal server instance {Instance.ServerId}");
                Shutdown();
                throw;
            }
        }

        public IUnrealServerInstance? GetByChannelId(Guid ChannelId)
        {
            lock (_ServerDescriptorLock)
            {
                return _ServerDescriptors.Values
                    .Where((d) => d.Channel != null && d.Channel.ChannelId == ChannelId)
                    .Select((d) => d.Instance)
                    .FirstOrDefault();
            }
        }

        public IUnrealServerInstance? GetByProgenitorGroupId(Guid GroupId)
        {
            lock (_ServerDescriptorLock)
            {
                return _ServerDescriptors.Values
                    .Where((d) => d.ProgenitorGroup != null && d.ProgenitorGroup.GroupId == GroupId)
                    .Select((d) => d.Instance)
                    .FirstOrDefault();
            }
        }

        public void SetChannel(IUnrealServerInstance Instance, MChannel Channel)
        {
            lock (_ServerDescriptorLock)
            {
                _ServerDescriptors[Instance.ServerId].Channel = Channel;
            }

            (Instance as UnrealServerInstance).SetChannelId(Channel.ChannelId);
        }


        public void Shutdown()
            => Dispose();


        public void Dispose()
        {
            if (!_Disposing)
            {
                _Disposing = true;
                lock (_ServerDescriptorLock)
                {
                    foreach (var descriptor in _ServerDescriptors.Values)
                    {
                        if (descriptor.ServerState != EUnrealServerState.ShuttingDown)
                            descriptor.Instance.Dispose();
                    }

                    _ServerDescriptors.Clear();
                }
            }
        }
    }

}
