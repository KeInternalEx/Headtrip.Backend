using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    /*
     * Sent by the server instance to create a server transfer request for a given player
     */
    public sealed class MsgRequestPlayerTransfer : AUnrealMessage
    {
        public static readonly string MsgType = "S.TRANSFER.PLAYER.REQUEST";
        public Guid AccountId { get; set; }
        public string ZoneName { get; set; }

        public MsgRequestPlayerTransfer(string zoneName, string accountId) :
            base(MsgType)
        {
            ZoneName = zoneName;
            AccountId = Guid.Parse(accountId);
        }
    }
}
