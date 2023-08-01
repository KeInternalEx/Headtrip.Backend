using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Objects.Abstract.Results;

namespace Headtrip.Objects.Account
{
    public sealed class RAccountCreationResult : AServiceCallResult
    {
        public MAccount? Account { get; set; }
    }
}
