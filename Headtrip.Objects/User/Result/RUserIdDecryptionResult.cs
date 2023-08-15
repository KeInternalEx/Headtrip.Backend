using Headtrip.Objects.Abstract.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.User.Result
{
    public sealed class RUserIdDecryptionResult : AServiceCallResult
    {
        public Guid UserId { get; set; }
    }
}
