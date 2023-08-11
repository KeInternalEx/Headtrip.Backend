using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    /*
     * Sent by the server instance to tell the service that a player has joined
     */
    public sealed class MsgPlayerJoin : AUnrealMessage
    {
        public static readonly string MsgType = "S.PLAYER.JOIN";

        public Guid AccountId { get; set; }
        public string PreviousZoneName { get; set; }

        public MsgPlayerJoin(string accountId, string previousZoneName) :
            base(MsgType)
        {
            AccountId = Guid.Parse(accountId);
            PreviousZoneName = previousZoneName;
        }
    }
}
