using Headtrip.UeService.UnrealEngine.Messaging.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Messaging
{
    public sealed class MessagePipeline : IMessagePipeline
    {
        private Dictionary<string, List<MessageHandler>> _handlers =
            new Dictionary<string, List<MessageHandler>>();

        public void AddMessageHandler<T>(MessageHandler handler)
        {
            if (!_handlers.ContainsKey(typeof(T).Name))
                _handlers.Add(typeof(T).Name, new List<MessageHandler>());

            _handlers[typeof(T).Name].Add(handler);
        }

        public void RemoveMessageHandler<T>(MessageHandler handler)
        {
            if (!_handlers.ContainsKey(typeof(T).Name))
                return;

            _handlers[typeof(T).Name].Remove(handler);
        }


        public int ProcessMessage(byte[] message)
        {
            int numHandlersInvoked = 0;
            var messageStr = Encoding.UTF8.GetString(message);
            var messageObject = JsonConvert.DeserializeObject<IMessageObject>(messageStr);

            if (messageObject == null)
                return -1;

            var messageType = Type.GetType(messageObject.MessageType);
            if (messageType == null)
                return -1;

            if (!_handlers.ContainsKey(messageObject.MessageType))
                return -1;

            var typedMessageObject = Activator.CreateInstance(messageType);
            if (typedMessageObject == null)
                return -1;

            foreach (var field in messageType.GetFields())
                field.SetValue(typedMessageObject, field.GetValue(messageObject));

            foreach (var handler in _handlers[messageObject.MessageType])
            {
                handler.Invoke((IMessageObject)typedMessageObject);
                numHandlersInvoked++;
            }


            return numHandlersInvoked;
        }

    }
}
