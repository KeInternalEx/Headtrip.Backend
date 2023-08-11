using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Headtrip.UeMessages;
using Headtrip.UeMessages.Outbound;
using Headtrip.UeService.UnrealEngine.MessageHandlers;

namespace Headtrip.UeService.UnrealEngine
{
    public sealed class UnrealMessageBus : IAsyncDisposable
    {
        private IPEndPoint _SendEndpoint;
        private IPEndPoint _ReceiveEndpoint;
        private Socket _SendSocket;
        private Socket _ReceiveSocket;

        private IEnumerable<IUnrealMessageHandler> _MessageHandlers;


        public string ListeningPort { get { return _ReceiveEndpoint.Port.ToString(); } }
        public string SendingPort { get {  return _SendEndpoint.Port.ToString(); } }


        public UnrealMessageBus()
        {
            _ReceiveEndpoint = new IPEndPoint(IPAddress.Loopback, 0);
            _SendEndpoint = new IPEndPoint(IPAddress.Loopback, 0);


            _ReceiveSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _ReceiveSocket.Bind(_ReceiveEndpoint);
            
            _SendSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _SendSocket.Bind(_SendEndpoint);

            _MessageHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => typeof(IUnrealMessageHandler).IsAssignableFrom(t))
                .Select((t) => Activator.CreateInstance(t) as IUnrealMessageHandler)
                .Where((h) => h != null)
                .Select((h) => h!);
        }

        public async Task SendMessage(AUnrealMessage Message)
            => await _SendSocket.SendAsync(Message.Serialize());

        public async ValueTask DisposeAsync()
        {
            if (_SendSocket != null)
            {
                await _SendSocket.SendAsync(new MsgShutdown().Serialize());
                await Task.Delay(15000); // Give the server 15 seconds to shutdown

                _SendSocket.Dispose();
            }

            if (_ReceiveSocket != null)
            {
                _ReceiveSocket.Dispose();
            }
        }


        /*
         *                 _Process = Process.Start(
                    ConfigurationManager.AppSettings["UnrealDedicatedServerPath"],
                    $"{_LevelName}?sp={_ReceiveEndpoint.Port}&rp={_SendEndpoint.Port} -server");
         */

    }
}
