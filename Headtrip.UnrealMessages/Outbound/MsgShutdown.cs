using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Outbound
{
    /*
     * Sent by the service to tell the server instance to close
     */
    public sealed class MsgShutdown : AUnrealMessage
    {
        public static readonly string MsgType = "U.SHUTDOWN";

        public MsgShutdown() :
            base(MsgType)
        {

        }
    }
}
