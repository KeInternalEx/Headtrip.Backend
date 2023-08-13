using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Headtrip.GameServerContext;
using Headtrip.UeMessages;
using Headtrip.UeMessages.Outbound;
using Headtrip.UeService.State;
using Headtrip.UeService.UnrealEngine.Interface;
using Headtrip.UeService.UnrealEngine.MessageHandlers.Interface;
using Headtrip.Utilities.Interface;
using Microsoft.Extensions.DependencyInjection;
using PeterO.Cbor;

namespace Headtrip.UeService.UnrealEngine
{
    public sealed class UnrealMessageBus : IUnrealMessagePoller, IAsyncDisposable
    {
        private readonly IPEndPoint _Endpoint;
        private readonly Socket _Socket;
        private readonly IEnumerable<IUnrealMessageHandler> _MessageHandlers;


        private readonly ILogging<HeadtripGameServerContext> _Logging;

        private bool _Initialized;

        public string Port { get { return _Endpoint.Port.ToString(); } }

        public UnrealMessageBus(
            UnrealServerInstance ServerInstance,
            IServiceProvider ServiceProvider,
            ILogging<HeadtripGameServerContext> Logging)
        {
            _Initialized = false;
            _Logging = Logging;


            _Endpoint = new IPEndPoint(IPAddress.Loopback, 0);

            _Socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _Socket.Bind(_Endpoint);

            _MessageHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => typeof(IUnrealMessageHandler).IsAssignableFrom(t))
                .Select((t) => Activator.CreateInstance(t, t
                    .GetConstructors()[0]
                    .GetParameters()
                    .Select((p) => ServiceProvider.GetRequiredService(p.ParameterType))) as IUnrealMessageHandler)
                .Where((h) => h != null)
                .Select((h) => h!);

            foreach (var handler in _MessageHandlers)
                handler.SetServerInstance(ServerInstance);

            if (!UeServiceState.UnrealMessagePollers.TryAdd(this.GetHashCode(), this))
                throw new Exception($"Unable to add message bus {this.GetHashCode()} to list of mesage pollers.");
        }


        private async Task<byte[]?> ReadSocketData(CancellationToken Token)
        {
            var buffer = new byte[1024];
            var byteArrays = new List<byte[]>();
            var messages = new List<AUnrealMessage>();

            while (!Token.IsCancellationRequested)
            {
                var bytesReceived = await _Socket.ReceiveAsync(buffer, SocketFlags.None, Token);
                if (bytesReceived == 0)
                    break;

                byteArrays.Add(buffer.Take(bytesReceived).ToArray());
            }

            var contiguousBufferSize = byteArrays.Sum((b) => b.Length);
            if (contiguousBufferSize == 0)
                return null;

            var contiguousBuffer = new byte[contiguousBufferSize];

            // Read all of the bytes from the separate reads into one contiguous buffer
            for (int i = 0, k = 0; i < byteArrays.Count; ++i)
            {
                var b = byteArrays[i];
                if (k + b.Length > contiguousBufferSize)
                    throw new Exception($"Error while combining buffers. Resulting write would overflow the contiguousBuffer");

                for (int j = 0; j < b.Length; ++j)
                    contiguousBuffer[j + k] = b[j];

                k += b.Length;
            }

            return contiguousBuffer;
        }

        private bool VerifyUnrealMessage(CBORObject CborObject)
        {
            return
                CborObject.Type == CBORType.Map &&
                CborObject.ContainsKey("Type") &&
                CborObject.ContainsKey("Version") &&
                CborObject.ContainsKey("HTMGX");
        }

        private async Task<List<AUnrealMessage>> ReadMessages(CancellationToken Token)
        {
            var result = new List<AUnrealMessage>();

            var socketData = await ReadSocketData(Token);
            if (socketData == null)
                return result;


            // socketData contains 1 or more CBOR objects
            // need to parse them into message objects

            using (var memStream = new MemoryStream(socketData))
            {
                while (
                    memStream.Position != memStream.Length &&
                    Token.IsCancellationRequested)
                {
                    var cborObject = CBORObject.Read(memStream);

                    if (!VerifyUnrealMessage(cborObject))
                    {
                        _Logging.LogWarning($"Received CBOR object that was not convertable to an AUnrealMessage object {cborObject.ToJSONString()}");
                        continue;
                    }

                    result.Add(JsonSerializer.Deserialize<AUnrealMessage>(cborObject.ToJSONString()));
                }
            }

            return result;
        }


        private async Task ReceiveMessage(AUnrealMessage Message, CancellationToken Token)
            => await Task.WhenAll(_MessageHandlers.Select((handler) => handler.HandleMessage(Message, Token)));

        public async Task Poll(CancellationToken Token)
            => await Task.WhenAll((await ReadMessages(Token)).Select((m) => ReceiveMessage(m, Token)));

        public async Task SendMessage(AUnrealMessage Message)
            => await _Socket.SendAsync(Message.SerializeCbor());

        public async ValueTask DisposeAsync()
        {
            if (_Initialized)
                UeServiceState.UnrealMessagePollers.TryRemove(this.GetHashCode(), out _);

            if (_Socket != null)
            {
                await SendMessage(new MsgShutdown());
                await Task.Delay(UeServiceConfiguration.ServerShutdownGracePeriod); 

                _Socket.Dispose();
            }
        }


    }
}
