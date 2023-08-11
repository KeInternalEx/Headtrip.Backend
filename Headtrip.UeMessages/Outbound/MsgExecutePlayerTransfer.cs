using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages
{
    /*
     * Sent by the service to move the given player from one server instance to another
     */
    public sealed class MsgExecutePlayerTransfer : AUnrealMessage
    {
        public static readonly string MsgType = "U.TRANSFER.PLAYER.EXEC";

        public string ConnectionString { get; set; }
        public Guid AccountId { get; set; }

        public MsgExecutePlayerTransfer(string connectionString, Guid accountId) :
            base(MsgType)
        {
            ConnectionString = connectionString;
            AccountId = accountId;
        }
    }
}
