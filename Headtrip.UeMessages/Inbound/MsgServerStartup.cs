using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    /*
     * Sent by the server instance to tell the service that it's running and what its connection string is
     */
    public sealed class MsgServerStartup : AUnrealMessage
    {
        public static readonly string MsgType = "S.SERVER.BEGIN";

        public string ConnectionString { get; set; }
        public MsgServerStartup(string connectionString) :
            base(MsgType)
        {
            ConnectionString = connectionString;
        }
    }
}
