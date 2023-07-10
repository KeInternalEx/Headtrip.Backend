using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Account
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid SelectedCharacterId { get; set; }


    }
}
