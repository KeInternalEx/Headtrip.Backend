using PeterO.Cbor;
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
        public readonly string HTMGX;

        protected AUnrealMessage(string type, string? version = null)
        {
            Type = type;
            Version = version ?? Assembly.GetExecutingAssembly().GetName().Version.ToString();
            HTMGX = string.Empty;
        }

        public string SerializeJson()
            => JsonSerializer.Serialize(this, this.GetType());

        public byte[] SerializeCbor()
        {
            using (var stream = new MemoryStream())
            {
                JsonSerializer.Serialize(stream, this, this.GetType(), JsonSerializerOptions.Default);
                return CBORObject.ReadJSON(stream, JSONOptions.Default).EncodeToBytes();
            }
        }
    }
}
