using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.IPCMessages
{
    public abstract class AIPC_MSG<T>
    {
        public string Type = typeof(T).Name;
        public string Encode() => JsonConvert.SerializeObject(this);
        public void Verify()
        {
            if (Type != typeof(T).Name)
                throw new Exception($"Expected APIC_MSG type {typeof(T).Name}, got type {Type}");
        }

        public static T? Decode(string s) => (T?)JsonConvert.DeserializeObject(s, typeof(T));
    }
}
