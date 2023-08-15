using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Objects.Abstract.Results;

namespace Headtrip.Objects.User.Result
{
    public sealed class RUserCreationResult : AServiceCallResult
    {
        public string? EmailConfirmationParameter { get; set; }
        public MUser? User { get; set; }
    }
}
