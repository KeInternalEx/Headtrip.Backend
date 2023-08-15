using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Headtrip.GameServerContext;
using Headtrip.Objects.UnrealService.Transient;
using Headtrip.UeMessages;
using Headtrip.UnrealService.State;
using Headtrip.UnrealService.UnrealEngine.Interface;
using Headtrip.Utilities;
using Headtrip.Utilities.Interface;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace Headtrip.UnrealService.UnrealEngine
{
    public sealed class UnrealServerInstance : IUnrealServerInstance, IDisposable
    {
        private readonly Guid _ServerId;
        private readonly string _LevelName;
        private readonly TStrGroup _ProgenitorGroup;


        private Guid? _ChannelId;
        private IUnrealMessageBus? _MessageBus;

        private UnrealProcess? _Process;

        private ILogging<HeadtripGameServerContext> _Logging;


        public Guid? ChannelId {  get {  return _ChannelId; } }
        public Guid ServerId { get {  return _ServerId; } }
        public string LevelName { get { return _LevelName; } }
        public string ZoneName { get { return _ProgenitorGroup.ZoneName!; } }

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

        public void SetMessageBus(IUnrealMessageBus MessageBus)
        {
            _MessageBus = MessageBus;
        }

        public async Task SendMessage(AUnrealMessage Message)
        {
            if (_MessageBus == null)
                throw new Exception($"Message bus not yet initialized for server instance {ServerId}");

            await _MessageBus.SendMessage(Message);
        }
        public void Start()
        { 
            _Process = new UnrealProcess(UnrealServiceConfiguration.ServerBinaryPath, $"{_LevelName}?ServiceSocketPort={_MessageBus.Port} -server");
        }

        public void SetChannelId(Guid ChannelId)
            => _ChannelId = ChannelId;

        public void Dispose()
        {
            if (_Process != null)
                _Process.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
