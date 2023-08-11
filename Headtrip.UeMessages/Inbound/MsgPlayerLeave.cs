using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    /*
     * Sent by the server instance to tell the service that a player has left
     */
    public sealed class MsgPlayerLeave : AUnrealMessage
    {
        public static readonly string MsgType = "S.PLAYER.LEAVE";

        public Guid AccountId { get; set; }

        public MsgPlayerLeave(string accountId) :
            base(MsgType)
        {
            AccountId = Guid.Parse(accountId);
        }
    }
}
