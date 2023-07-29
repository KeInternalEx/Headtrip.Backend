using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.UnrealEngine.Messaging.Abstract
{
    public abstract class AMessageObject<T> : IMessageObject
    {
        public string MessageType { get; set; } = typeof(T).Name;
        public string Serialize() { return JsonConvert.SerializeObject(this); }

    }
}
