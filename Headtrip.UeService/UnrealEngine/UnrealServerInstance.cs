using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;


namespace Headtrip.UeService.UnrealEngine
{
    /**
     * DO NOT STORE REFERENCES TO THIS OBJECT OUTSIDE OF THE UeServiceState DICTS
     */
    public sealed class UnrealServerInstance : IAsyncDisposable
    {
        public readonly string _LevelName;
        public string LevelName { get { return _LevelName; } }

        private UnrealProcess _Process;
        private UnrealMessageBus _MessageBus;



        public UnrealServerInstance(string LevelName)
        {
            _LevelName = LevelName;

            _MessageBus = new UnrealMessageBus();
            _Process = new UnrealProcess(
                ConfigurationManager.AppSettings["UnrealDedicatedServerPath"],
                $"{_LevelName}?sp={_MessageBus.ListeningPort}&rp={_MessageBus.SendingPort} -server");
        }


        public async Task<Guid> SetChannelId(Guid ChannelId)
        {
            _ChannelId = ChannelId;

            // TODO: NEED TO SEND A CHANNEL ID UPDATE COMMAND TO THE UE SERVER INSTANCE


            return _ChannelId.Value;
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
