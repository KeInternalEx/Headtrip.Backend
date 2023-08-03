using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Headtrip.UeService.UnrealEngine.Messaging;
using Headtrip.UeService.UnrealEngine.Messaging.Abstract;
using Headtrip.UeService.UnrealEngine.Messaging.Messages.ToUnreal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;


namespace Headtrip.UeService.UnrealEngine
{
    /**
     * DO NOT STORE REFERENCES TO THIS OBJECT OUTSIDE OF THE UeServiceState DICTS
     */
    public sealed class UnrealServerInstance : IDisposable, IAsyncDisposable
    {
        private bool _Disposed;

        public readonly string _LevelName;
        public string LevelName { get { return _LevelName; } }

        private string? _ConnectionString;
        public string? ConnectionString { get { return _ConnectionString; } }

        private Guid? _ChannelId;
        public Guid? ChannelId { get { return _ChannelId; } }

        private bool _IsRunning;
        public bool IsRunning { get { return _IsRunning; } }

        private Process? _Process;
        private IPEndPoint? _SendEndpoint;
        private IPEndPoint? _ReceiveEndpoint;
        private Socket? _SendSocket;
        private Socket? _ReceiveSocket;

        public UnrealServerInstance(string LevelName)
        {
            _LevelName = LevelName;
            _IsRunning = false;
        }

        public Task Begin()
        {
            try
            {
                _SendEndpoint = new IPEndPoint(IPAddress.Loopback, 0);
                _ReceiveEndpoint = new IPEndPoint(IPAddress.Loopback, 0);

                _SendSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                _SendSocket.Bind(_SendEndpoint);

                _ReceiveSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                _ReceiveSocket.Bind(_ReceiveEndpoint);



                _Process = Process.Start(
                    ConfigurationManager.AppSettings["UnrealDedicatedServerPath"],
                    $"{_LevelName}?sp={_ReceiveEndpoint.Port}&rp={_SendEndpoint.Port} -server");

                _IsRunning = true;


                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }


        public async Task<Guid> SetChannelId(Guid ChannelId)
        {
            _ChannelId = ChannelId;

            // TODO: NEED TO SEND A CHANNEL ID UPDATE COMMAND TO THE UE SERVER INSTANCE


            return _ChannelId.Value;
        }



        private async Task Close()
        {

        }


        private async ValueTask DisposeAsyncCore()
        {
            if (_Process != null &&
                _SendSocket != null &&
                _ReceiveSocket != null)
            {
                await Close();
            }
        }


        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_Disposed)
                return;

            if (disposing)
            {
                if (_Process != null)
                {
                    _Process.Close();
                    _Process.Dispose();
                    _Process = null;
                }

                if (_ReceiveSocket != null)
                {
                    _ReceiveSocket.Close();
                    _ReceiveSocket.Dispose();
                    _ReceiveSocket = null;
                }

                if (_SendEndpoint != null)
                {
                    _SendSocket.Close();
                    _SendSocket.Dispose();
                    _SendSocket = null;
                }
            }

            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
