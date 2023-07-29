using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Headtrip.UeService.UnrealEngine.Abstract;
using Headtrip.UeService.UnrealEngine.Messaging;
using Headtrip.UeService.UnrealEngine.Messaging.Abstract;
using Headtrip.UeService.UnrealEngine.Messaging.Messages.ToUnreal;
using Newtonsoft.Json;


namespace Headtrip.UeService.UnrealEngine
{
    public sealed class UnrealServerInstance : IUnrealServerInstance
    {
        private bool disposedValue;


        private readonly string? _filePath;
        private readonly Guid _channelId;

        private IMessagePipeline _messagePipeline;
        private Process _process;
        private IPEndPoint _endPoint;
        private Socket _socket;

        public UnrealServerInstance(
            Guid UeServiceId,
            Guid channelId,
            string LevelName)
        {
            _filePath = ConfigurationManager.AppSettings["UnrealServerBinaryPath"];
            if (_filePath == null)
                throw new Exception($"UnrealServerBinaryPath not set in app settings for UeService {UeServiceId}");

            _channelId = channelId;
            _endPoint = new IPEndPoint(IPAddress.Loopback, 0);

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(_endPoint);
            _socket.Listen();

            _process = Process.Start(_filePath, $"{LevelName}?UeServiceIpcPort={_endPoint.Port} -server");
            _messagePipeline = new MessagePipeline();
        }


        public async Task WriteString(string text)
        {
            await _socket.SendAsync(Encoding.UTF8.GetBytes(text));
        }

        public async Task WriteObject(IMessageObject messageObject)
        {
            var str = JsonConvert.SerializeObject(messageObject);
            var buf = Encoding.UTF8.GetBytes(str);

            await _socket.SendAsync(buf);
        }

        public void AddMessageHandler<T>(MessageHandler handler) =>
            _messagePipeline.AddMessageHandler<T>(handler);

        public void RemoveMessageHandler<T>(MessageHandler handler) =>
            _messagePipeline.RemoveMessageHandler<T>(handler);









        #region Dispose
        public void Dispose()
        {
            if (!disposedValue)
            {
                if (_process != null)
                {
                    _process.Kill();
                    _process.Dispose();
                }
                disposedValue = true;
            }
        }
        public async ValueTask DisposeAsyncCore()
        {
            if (_socket != null)
            {
                await WriteObject(new MsgShutdownServer());
            }
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion Dispose

    }
}
