using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.User
{
    public class UserIdDecryptionResult : ServiceCallResult
    {
        public Guid UserId { get; set; }
    }
}
