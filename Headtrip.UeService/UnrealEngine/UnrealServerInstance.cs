using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Headtrip.GameServerContext;
using Headtrip.Objects.UeService;
using Headtrip.UeService.State;
using Headtrip.Utilities;
using Headtrip.Utilities.Interface;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace Headtrip.UeService.UnrealEngine
{
    /**
     * DO NOT STORE REFERENCES TO THIS OBJECT OUTSIDE OF THE UeServiceState DICTS
     */
    public sealed class UnrealServerInstance : IAsyncDisposable
    {
        private readonly Guid _ServerId;
        private readonly string _LevelName;
        private readonly TStrGroup _ProgenitorGroup;


        private UnrealProcess? _Process;
        private UnrealMessageBus? _MessageBus;

        private ILogging<HeadtripGameServerContext> _Logging;


        public Guid ServerId {  get {  return _ServerId; } }
        public string LevelName { get { return _LevelName; } }
        public TStrGroup ProgenitorGroup { get { return _ProgenitorGroup; } }

        public UnrealServerInstance(
            string LevelName,
            TStrGroup ProgenitorGroup,
            ILogging<HeadtripGameServerContext> Logging)
        {
            _Logging = Logging;

            _ServerId = Guid.NewGuid();
            _LevelName = LevelName;
            _ProgenitorGroup = ProgenitorGroup;
        }

        public void Start(IServiceProvider ServiceProvider)
        {
            _MessageBus = new UnrealMessageBus(this, ServiceProvider, _Logging);
            _Process = new UnrealProcess(UeServiceConfiguration.ServerBinaryPath, $"{_LevelName}?ServiceSocketPort={_MessageBus.Port} -server");
        }


        public async ValueTask DisposeAsync()
        {

            if (_MessageBus != null)
                await _MessageBus.DisposeAsync();

            if (_Process != null)
                _Process.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
