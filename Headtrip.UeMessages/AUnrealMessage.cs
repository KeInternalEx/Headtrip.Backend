using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headtrip.UeMessages
{
    public abstract class AUnrealMessage
    {
        public readonly string Type;
        public readonly string Version;

        protected AUnrealMessage(string type, string? version = null)
        {
            Type = type;
            Version = version ?? Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public byte[] Serialize()
            => Encoding.Unicode.GetBytes(JsonSerializer.Serialize(this, this.GetType()));
    }
}
