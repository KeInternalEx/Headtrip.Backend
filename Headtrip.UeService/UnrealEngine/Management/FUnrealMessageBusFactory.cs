using Headtrip.GameServerContext;
using Headtrip.UeService.UnrealEngine.Interface;
using Headtrip.UeService.UnrealEngine.Management.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Management
{
    public sealed class FUnrealMessageBusFactory : IUnrealMessageBusFactory, IAsyncDisposable
    {
        private Dictionary<Guid, TUnrealMessageBusDescriptor> _MessageBusses
            = new Dictionary<Guid, TUnrealMessageBusDescriptor>();

        private object _MessageBussesLock = new object();
        private bool _Disposing = false;

        private IServiceProvider _ServiceProvider;
        private ILogging<HeadtripGameServerContext> _Logging;

        public FUnrealMessageBusFactory(
            IServiceProvider ServiceProvider,
            ILogging<HeadtripGameServerContext> Logging)
        {
            _ServiceProvider = ServiceProvider;
            _Logging = Logging;

        }

        public IUnrealMessageBus Create(IUnrealServerInstance ServerInstance)
        {
            if (_Disposing)
                throw new Exception("Cannot create unreal message bus, factory is disposing.");


            var messageBus = new UnrealMessageBus(ServerInstance, _ServiceProvider, _Logging);
            var messageBusDescriptor = new TUnrealMessageBusDescriptor
            {
                ServerInstance = ServerInstance,
                MessageBus = messageBus,
                State = EUnrealMessageBusState.Started
            };

            lock (_MessageBussesLock)
            {
                _MessageBusses[messageBus.MessageBusId] = messageBusDescriptor;
            }


            return messageBus;

        }

        public IEnumerable<IUnrealMessagePoller> GetPollers()
        {
            lock (_MessageBussesLock)
            {
                return _MessageBusses.Values
                    .Where((d) => d.MessageBus != null && d.State == EUnrealMessageBusState.Started)
                    .Select((d) => d.MessageBus! as IUnrealMessagePoller);
            }
        }

        public IUnrealMessageBus? GetByServerId(Guid ServerId)
        {
            lock (_MessageBussesLock)
            {
                return _MessageBusses.Values
                    .Where((d) => d.ServerInstance.ServerId == ServerId && d.State != EUnrealMessageBusState.ShuttingDown && d.MessageBus != null)
                    .Select((d) => d.MessageBus! as IUnrealMessageBus)
                    .FirstOrDefault();
            }
        }

        public async Task DestroyMessageBus(IUnrealMessageBus MessageBus)
        {
            try
            {
                lock (_MessageBussesLock)
                {
                    if (_MessageBusses[MessageBus.MessageBusId].State == EUnrealMessageBusState.ShuttingDown)
                        return;

                    _MessageBusses[MessageBus.MessageBusId].State = EUnrealMessageBusState.ShuttingDown;
                }

                await (MessageBus as UnrealMessageBus).DisposeAsync();

                lock (_MessageBussesLock)
                {
                    _MessageBusses.Remove(MessageBus.MessageBusId);
                }

            }
            catch (Exception ex)
            {
                _Logging.LogException(ex, $"Fatal exception while attempting to terminate message bus {MessageBus.MessageBusId}");
                await Shutdown();
                throw;
            }
        }


        public async Task Shutdown()
            => await DisposeAsync();

        public async ValueTask DisposeAsync()
        {
            if (!_Disposing)
            {
                _Disposing = true;

                var tasks = new List<Task>();
                lock (_MessageBussesLock)
                {
                    foreach (var messageBus in _MessageBusses.Values)
                    {
                        if (messageBus.State != EUnrealMessageBusState.ShuttingDown)
                            tasks.Add(messageBus.MessageBus.DisposeAsync().AsTask());
                    }

                    _MessageBusses.Clear();
                }

                await Task.WhenAll(tasks);
            }
        }

    }
}
